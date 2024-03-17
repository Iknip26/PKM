using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;


public class gameSystem : MonoBehaviour
{

    [System.Serializable]
    public class dataGame{
        public Sprite gambar;
        public string nama;
    }

    [Header("Komponen, Asset dan Atributnya, dll")]

    public GameObject objekBaru, tempatDrop;
    public TextMeshProUGUI txtLevel, txtWaktu, txtScore;

    public GameObject txtWin;

    public HandTracking handTracking;

    public SpriteRenderer gambarTebakan;

    public dataGame[] arrayDataGame;

    public float posSpawnX = -15.37f, posSpawnY = 6, ukuranDefault = 0.25f;

    public int syaratMenang;

    public bool apakahGameAktif = true;

    private dataGame dataTerpilih;
    private bool isPaused;

    public List<GameObject> listWordBox = new List<GameObject>(),  listTempatDrop = new List<GameObject>();



    [Space]

    [Header("Data Game")]

    public int dataLevel = 0, dataWaktu = 0, dataScore = 0;
    public static gameSystem instance;



    private void Awake() {
        instance = this;
    }

    void SpawnObject(int ID_ITEM, string[] arrayDataTerpilih, float posSpawnX, float posSPawnY, Quaternion rotasiSpawn)
    {
        if (objekBaru != null)
        {

            double tmpPengali = (double)arrayDataTerpilih.Length / 2;
            // print(arrayDataTerpilih.Length + "dibagi 2 =" + tmpPengali);

            tmpPengali = Math.Ceiling(tmpPengali);
            // print("tmppengali dibulatkan ke atas =" + tmpPengali);

            float pengaliUkuran = (float)tmpPengali - 2;
            // print("pengaliUkuran =" + pengaliUkuran);


            // INISIASI OBJEK BARU
            GameObject newObjek = Instantiate(objekBaru, new Vector3(posSpawnX + ( (2 * ID_ITEM) - 0.5f * pengaliUkuran), posSpawnY-1), rotasiSpawn);
            objekBaru.SetActive(true);
           
            // Set ATribut Objek Baru
            
            newObjek.transform.localScale = new Vector3(1.25f - 0.25f * pengaliUkuran, 1.25f - 0.25f * pengaliUkuran);
            wordBox scriptObjekBaru = newObjek.GetComponent<wordBox>();
            Canvas canvas = newObjek.GetComponentInChildren<Canvas>();
            TextMeshProUGUI isiTextBaru = canvas.GetComponentInChildren<TextMeshProUGUI>();
            isiTextBaru.text = arrayDataTerpilih[ID_ITEM];

            // GameObject gameManager = GameObject.FindWithTag("gameManager");
            // HandTracking handTracking = gameManager.GetComponent<HandTracking>();

            scriptObjekBaru.handTracking = handTracking;
            scriptObjekBaru.ID_ITEM = ID_ITEM + 1;
            scriptObjekBaru.isiHuruf = arrayDataTerpilih[ID_ITEM];

            listWordBox.Add(newObjek);
        }
        else
        {
            Debug.LogError("Prefab objek baru belum ditentukan!");
        }
    }

    void SpawnTeampatDrop(int ID_ITEM, string[] arrayDataTerpilih, float posSpawnX, float posSPawnY, Quaternion rotasiSpawn)
    {
        if (tempatDrop != null)
        {

            double tmpPengali = (double)arrayDataTerpilih.Length / 2;
            // print(arrayDataTerpilih.Length + "dibagi 2 =" + tmpPengali);

            tmpPengali = Math.Ceiling(tmpPengali);
            // print("tmppengali dibulatkan ke atas =" + tmpPengali);

            float pengaliUkuran = (float)tmpPengali - 2;
            // print("pengaliUkuran =" + pengaliUkuran);


            // INISIASI OBJEK BARU
            GameObject newTempatDrop = Instantiate(tempatDrop, new Vector3((posSpawnX) + ( (2 * ID_ITEM) - 0.5f * pengaliUkuran), posSpawnY - 3), rotasiSpawn);
            newTempatDrop.SetActive(true);
           
            // Set ATribut Objek Baru
            
            tempatDrop scriptTempatDropBaru = newTempatDrop.GetComponent<tempatDrop>();
            scriptTempatDropBaru.ID_ITEM = ID_ITEM + 1;
            scriptTempatDropBaru.isiHuruf = arrayDataTerpilih[ID_ITEM];
            listTempatDrop.Add(newTempatDrop);
        }

        else
        {
            Debug.LogError("Prefab objek baru belum ditentukan!");
        }
    }

    void setInfoUi(){
        txtWaktu.text = (dataLevel + 1).ToString();
        txtScore.text = dataScore.ToString();
        txtLevel.text = dataLevel.ToString();

        int menit = UnityEngine.Mathf.FloorToInt(dataWaktu / 60);
        int detik = UnityEngine.Mathf.FloorToInt(dataWaktu % 60);

        txtWaktu.text = menit.ToString("00") + ":" + detik.ToString("00");

    }

    static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T nilai = array[k];
            array[k] = array[n];
            array[n] = nilai;
        }
    }

    void acakSoal(){
        dataTerpilih = arrayDataGame[UnityEngine.Random.Range(0, arrayDataGame.Length)];
        
        gambarTebakan.sprite = dataTerpilih.gambar;
        string[] arraydataTerpilih = dataTerpilih.nama.Select(c => c.ToString()).ToArray();
        string[] arraydataTerpilihAcak = dataTerpilih.nama.Select(c => c.ToString()).ToArray();
        syaratMenang = arraydataTerpilih.Length;
        print(syaratMenang);
        ShuffleArray(arraydataTerpilihAcak);

        for(int i=0; i<arraydataTerpilihAcak.Length; i++){
            SpawnObject(i, arraydataTerpilihAcak, posSpawnX, posSpawnY, Quaternion.identity);
        }

         for(int i=0; i<arraydataTerpilih.Length; i++){
            SpawnTeampatDrop(i, arraydataTerpilih, posSpawnX, posSpawnY, Quaternion.identity);
        }
    }

    void cekScore(){

    }

     void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    // Method untuk menunda permainan
    void PauseGame()
    {
        Time.timeScale = 0f; // Mengatur timescale ke 0 untuk menunda permainan
    }

    // Method untuk melanjutkan permainan
    void ResumeGame()
    {
        Time.timeScale = 1f; // Mengatur timescale ke 1 untuk melanjutkan permainan
    }

    void Start()
    {
        acakSoal();
    }


    float tmpWaktu;

    void Update()
    {
        setInfoUi();

        if(apakahGameAktif){
            if (dataWaktu > 0)
            {
                tmpWaktu += Time.deltaTime;
                if(tmpWaktu>=1){
                    dataWaktu--;
                    tmpWaktu = 0;
                }
            }
        }

        print(syaratMenang);

        if(syaratMenang <= 0){
            foreach(GameObject isi in listWordBox){
                Destroy(isi);
            }
            foreach(GameObject isi in listTempatDrop){
                Destroy(isi);
            }
            acakSoal();
            dataLevel++;
        }
    }
} 
