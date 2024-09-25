using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class ScpJson : MonoBehaviour
{

    //Variaveis de ativação menu
    public VideoPlayer videoPlayer;
    public GameObject menuInicial, menuOptions, rawImage;
    public AudioSource audioSelecao;
    private Animator animatorRawImage;
    public string novaScena;

    //Variavel para salvar configurações
    public TMP_Dropdown resolucaoControler, qualidadeControler;
    public TMP_InputField textFPSControler;
    public Toggle limitarFPSControler, modoJanelaControler, autoSaveControler;
    public Slider geralVolumeControler, musicaVolumeControler, efeitosVolumeControler;


    private void Start()
    {
        animatorRawImage = rawImage.GetComponent<Animator>();
        rawImage.SetActive(false);
        menuInicial.SetActive(false);
        menuOptions.SetActive(false);
        CarregarSave();
        
       
    }
    void Update()
    {

        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            rawImage.SetActive(true);
            menuInicial.SetActive(true);
            audioSelecao.Play();
            videoPlayer.Play();
            animatorRawImage.SetTrigger("trasicao");
        }
        if (Input.GetKey(KeyCode.T))
        {
            SaveConfig();
        }
        if (Input.GetKey(KeyCode.Y))
        {
            CarregarSave();
        }
    }
    public void Options()
    {
        CarregarSave();
        menuInicial.SetActive(false);
        menuOptions.SetActive(true);
    }

    public void RetornarMenu()
    {
        menuInicial.SetActive(true);
        menuOptions.SetActive(false);
    }
    public void RetornarMenuESalvar()
    {
        SaveConfig();
        CarregarSave();
        RetornarMenu();
    }

    public void SaveConfig()
    {

        //resolucao
        var resolutionModelo = new Resolucao();
        switch (resolucaoControler.value)
        {
            case 0:
                resolutionModelo.width = 3440;
                resolutionModelo.height = 1440;
                break;
            case 1:
                resolutionModelo.width = 1920;
                resolutionModelo.height = 1080;
                break;
            case 2:
                resolutionModelo.width = 1024;
                resolutionModelo.height = 768;
                break;
            default:
                Debug.LogWarning("Resolução desconhecida. Valor padrão será usado.");
                break;
        }
        
        var config = new ScpSavesConfig()
        {
            //Auto save
            autoSave = autoSaveControler.isOn,

            //Resolução
            modoJanela = modoJanelaControler.isOn,
            resolucao = resolutionModelo,

            //volume
            geralVolume = geralVolumeControler.value,
            efeitosVolume = efeitosVolumeControler.value,
            musicaVolume = musicaVolumeControler.value,

            //Fps
            
            limiteFPS = new LimiteFPS()
            {
                fps = int.Parse(textFPSControler.text),
                limitar = limitarFPSControler.isOn
            },
            
            //Qualidade
            qualidade = (Qualidade)qualidadeControler.value
        };
        
        string json = "";

        // Define o caminho para salvar o arquivo JSON
        string filePath = Path.Combine(Application.persistentDataPath, "saves_jeisola" + ".json");

        Debug.Log("Salvando configurações em: " + filePath);



        // Verifica se o diretório existe, se não, cria-o
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        if (!File.Exists(filePath))
        {
            json = JsonUtility.ToJson(config, true);

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine(json);
            }
        }
        else
        {
            print("SaveState já existe. Sobrescrevendo save antigo.");

            File.Delete(filePath);

            json = JsonUtility.ToJson(config, true);

            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine(json);
            }
        }
        
        
    }

    public void CarregarSave()
    {
        // Use o mesmo caminho de `SaveConfig`
        string filePath = Path.Combine(Application.persistentDataPath, "saves_jeisola" + ".json");

        // Verificar se o arquivo existe
        if (File.Exists(filePath))
        {
            //Ler caminho
            string json = File.ReadAllText(filePath);
            //desserializar o JSON
            ScpSavesConfig config = JsonUtility.FromJson<ScpSavesConfig>(json);


            //Auto save
            autoSaveControler.isOn = config.autoSave;
            //Resolução
            modoJanelaControler.isOn = config.modoJanela;
            
            Debug.Log(resolucaoControler);
            var option = resolucaoControler.options.FirstOrDefault(x => x.text == $"{config.resolucao.width} x {config.resolucao.height}");


            if (option != null)
            {
                resolucaoControler.value = resolucaoControler.options.IndexOf(option);
            }
            else
            {
                Debug.LogWarning("Resolução salva não encontrada no dropdown.");

            }
            
            //fps
            textFPSControler.text = config.limiteFPS.fps.ToString();
            limitarFPSControler.isOn = config.limiteFPS.limitar;
            //Audio
            geralVolumeControler.value = config.geralVolume;
            musicaVolumeControler.value = config.musicaVolume;
            efeitosVolumeControler.value = config.efeitosVolume;
            //Qualidade
            qualidadeControler.value = (int)config.qualidade;


            Debug.Log("Configurações carregadas com sucesso.");

        }

        else
        {
            Debug.LogWarning("Arquivo de configurações não encontrado.");
        }
    }
    
   
}