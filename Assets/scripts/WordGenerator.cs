using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour
{

    public static bool pickFromA = true;

    private static List<string> a = new List<string>{"koulutussopimus","kurssi","lukukausi","lukuvuosi","lukuvuosisuunnitelma","opintojakso",
        "opintoviikko","opiskelijavalinta","opiskelupaikka","oppimiskokonaisuus","oppitunti","poissaolo","päivänavaus","pääsykoe","soveltuvuuskoe",
        "todistusvalinta","toimintakulttuuri","työjärjestys","uusintakoe","rästikoe","vapautus","vuosiluokka","välitunti","yhteishaku",
        "koulukiusaaminen","koulukuljetus","koulumatkatuki","koulumotivaatio","kouluruokailu","kouluterveydenhuolto","kouluviihtyvyys","kouluyhteisö",
        "opintotuki","syrjäytyminen","tukitoimi","äidinkieli","ruotsi","englanti","saksa","espanja","matematiikka","fysiikka","kemia","biologia",
        "maantieto","psykologia","terveystieto","liikunta","harrastus","luento","yliopisto","peruskoulu","lukio","ammattikoulu","korkeakoulu",
        "tietokone","koulu","opettaja","rehtori","vararehtori","alakoulu","yläkoulu","penkkarit","koeviikko","opintojakso","kesäloma","talviloma",
        "syysloma","oppilas","monivalinta","soveltaminen","koulubussi","koulutaksi","taloustieto","koulumatka","itsevarmuus","luovuus","minäkuva",
        "uskonto","haaveammatti","tulevaisuus","käytävä","ergonomia","välikoe","opo","testi","stressi","vastuu","preppaustunti","yleissivistys",
        "terminologia","opetussuunnitelma","musiikki","kuvaamataito","wilma","wilmamerkintä","kurssitarjotin","ohjelmointi","kouluruoka","kahvi",
        "abiturientti","essee","kirjoitelma","tutkielma","työrauha","sanakoe","vihko","penaali","kynä","kumi","pulpetti","luokkahuone","kesätyö",
        "työhaastattelu","opinnäytetyö","näppäintaidot","ammatti","arvosana","terveys","mielenterveys","joululoma","viikonloppu","mopo","mopoauto",
        "mallisuoritus","pitkäpinnaisuus","päättäväisyys","etäopiskelu","tentti","opintosuunnitelma","lähiopetus","tutkinto","opintokokonaisuus",
        "opintopiste","pääaine","referaatti","todistus","tuutori","koulutusohjelma","lehtori","metodologia","väitöskirja","professori","liitutaulu",
        "tussitaulu","kotitehtävä","oikeinkirjoitus","kausiloma","tukioppilas","koulutus","kirjasto","luentosali","kieliluokka","lokero","asuntola",
        "seminaari","ylioppilas","ylioppilaskirjoitukset","opettajainhuone","kirja","oppikirja"};   //tämä lista sisältää kaikki ne sanat joita peli saattaa kysyä


    private static List<string> b = new List<string>(a.Count);

    public static string GetRandomWord()
    {
        //valitsee satunnaisen sanan yläpuolelta ja palauttaa sen
        int randomIndex;
        string output;

        if (a.Count >= 1 && pickFromA == true)
        {
            randomIndex = Random.Range(0, a.Count);
            output = a[randomIndex];
            b.Add(output);
            a.Remove(output);

            if(a.Count == 0)
            {
                pickFromA = false;
            }
        }
        else
        {
            randomIndex = Random.Range(0, b.Count);
            output = b[randomIndex];
            a.Add(output);
            b.Remove(output);
            if(b.Count == 0)
            {
                pickFromA = true;
            }
        }
        return output;
    }
}
