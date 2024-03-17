using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class wordBox : MonoBehaviour
{
    public bool masukDropArea, masukAreaTangan, sedangDiPegang;
    public HandTracking handTracking;
    private gameSystem gameSystem;
    private Vector3 startPos, ukuranAwal;
    private GameObject tempatDrop;
    private BoxCollider2D boxCol;
    public bool apakahColliderTerResize, sudahTambahSkor = false;
    public int ID_ITEM;
    public string isiHuruf;





    void Start()
    {
        startPos = transform.position;
        ukuranAwal = transform.localScale;
        boxCol = handTracking.tangan.GetComponent<BoxCollider2D>();
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        gameSystem = gameManager.GetComponent<gameSystem>();
    }

    void Update()
    {
      
        if (masukAreaTangan && handTracking.pose == "grab")
        {
            if(handTracking.ID_ITEM_DIPEGANG == 0){
                handTracking.ID_ITEM_DIPEGANG = ID_ITEM;
            }

            if(ID_ITEM == handTracking.ID_ITEM_DIPEGANG){
                sedangDiPegang = true;
                transform.position = handTracking.grabPos.position;
                if (!apakahColliderTerResize && boxCol != null)
                {
                    Vector2 newSize = new Vector2(boxCol.size.x + 3f, boxCol.size.y + 3f);
                    boxCol.size = newSize;
                    apakahColliderTerResize = true;
            }
            }
        }
        else
        {
            sedangDiPegang = false;
            if (apakahColliderTerResize && boxCol != null)
            {
                handTracking.ID_ITEM_DIPEGANG = 0;
                Vector2 newSize = new Vector2(boxCol.size.x - 3f, boxCol.size.y - 3f);
                boxCol.size = newSize;
                apakahColliderTerResize = false;
            }
        }
        
        

        if (!sedangDiPegang)
        {
            if (masukDropArea)
            {
                if(isiHuruf == tempatDrop.transform.GetComponent<tempatDrop>().isiHuruf){
                    transform.localPosition = tempatDrop.transform.localPosition;
                    transform.localScale = tempatDrop.transform.localScale;
                    if(tempatDrop.transform.GetComponent<tempatDrop>().sudahTerisi == false){
                        gameSystem.syaratMenang--;
                        tempatDrop.transform.GetComponent<tempatDrop>().sudahTerisi = true;
                        if(sudahTambahSkor == false){
                            gameSystem.dataScore += 10;
                            sudahTambahSkor = true;
                        }
                    }
                
            }  else
            {
                transform.position = startPos;
                transform.localScale = ukuranAwal;
            }
            }
            else
            {
                // if(tempatDrop.transform.GetComponent<tempatDrop>().sudahTerisi == true){
                //     gameSystem.syaratMenang++;
                //     tempatDrop.transform.GetComponent<tempatDrop>().sudahTerisi = false;
                // }

                transform.position = startPos;
                transform.localScale = ukuranAwal;
            }
        }



    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("droparea"))
        {
            tempatDrop = trigger.gameObject;
            masukDropArea = true;
        }
        if (trigger.gameObject.CompareTag("tangan"))
        {
            masukAreaTangan = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("droparea"))
        {
            masukDropArea = false;
        }
        if (trigger.gameObject.CompareTag("tangan"))
        {
            masukAreaTangan = false;
        }
    }
}
