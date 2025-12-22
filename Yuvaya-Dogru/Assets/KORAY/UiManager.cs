using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject anaMenu, anaMenuOgeleri, nasilOynanirMenusu, yapimcilarMenusu, ayarlarMenusu,
                      pausePaneli, pauseOgeleri, pauseAyarlarMenusu, kazandinMenusu;
    private void Start()
    {
        anaMenu.SetActive(true);
        anaMenuOgeleri.SetActive(true);
        nasilOynanirMenusu.SetActive(false);
        yapimcilarMenusu.SetActive(false);
        ayarlarMenusu.SetActive(false);
        pausePaneli.SetActive(false);
        pauseOgeleri.SetActive(false);
        pauseAyarlarMenusu.SetActive(false);
        kazandinMenusu.SetActive(false);
    }
    public void OyunaSifirdanBasla()
    {
        anaMenu.SetActive(false);
        anaMenuOgeleri.SetActive(false);
        // Oyunun sýfýrdan(level 1) baþlamasý, kayýdýn sýfýrlanmasý ve oyun içi ui buraya kodlanacak
    }
    public void OyunaKaldiginYerdenDevamEt()
    {
        anaMenu.SetActive(false);
        anaMenuOgeleri.SetActive(false);
        // Oyunun kaldýðýmýz yerden baþlatýlmasý ve oyun içi ui buraya kodlanacak
    }
    public void YapimcilarMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        yapimcilarMenusu.SetActive(true);
    }
    public void AyarlarMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        ayarlarMenusu.SetActive(true);
    }
    public void NasilOynanirMenusuneGecis()
    {
        anaMenuOgeleri.SetActive(false);
        nasilOynanirMenusu.SetActive(true);
    }
    public void AnaMenuyeGecis()
    {
        if(nasilOynanirMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            nasilOynanirMenusu.SetActive(false);
        }
        else if(yapimcilarMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            yapimcilarMenusu.SetActive(false);
        }
        else if(ayarlarMenusu.activeSelf == true)
        {
            anaMenuOgeleri.SetActive(true);
            ayarlarMenusu.SetActive(false);
        }
        else if(pausePaneli.activeSelf == true)
        {
            pausePaneli.SetActive(false);
            pauseOgeleri.SetActive(false);
            anaMenu.SetActive(true);
            anaMenuOgeleri.SetActive(true);
            //oyun içi ui'in kapatilmasý falan buraya yazýlacak
        }
        else if(kazandinMenusu.activeSelf == true)
        {
            kazandinMenusu.SetActive(false);
            anaMenu.SetActive(true);
            anaMenuOgeleri.SetActive(true);
            //oyun içi'in kapatýlmasý falan buraya yazýlacak
        }
    }

    //pause ekranýna tuþla mý yoksa ui'daki buton ile mi geçilecek belli deðil
    //bu yüzden pause panelinin açýlýþýný kodlamadým
    //yalnýzca pause panelindeki butonlarýn fonksiyonlarýný yazdým
    public void BolumuYenidenOyna()
    {
        //en son kayýt edilen bölüm burada çaðýrýlacak
    }
    public void OyunaDevamEtPauseEkranindanCik()
    {
        pauseOgeleri.SetActive(false);
        pausePaneli.SetActive(false);
        //ui falan geri açýlacak ise buraya yazýlacak
    }
    public void PausedakiAyarlarMenusuneGecis()
    {
        pauseOgeleri.SetActive(false);
        pauseAyarlarMenusu.SetActive(true);
    }
    public void PauseAyarlardanDuzPaneleGeriGecme()
    {
        pauseOgeleri.SetActive(true);
        pauseAyarlarMenusu.SetActive(false);
    }
    //kazandin menüsünün çaðrýlmasý levelda olacak o yüzden çaðýrmayý yazmadým
    //sadece o menüden ana menüye'ye geçiren butonu kodladým
}
