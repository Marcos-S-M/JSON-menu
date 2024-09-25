using System;

[Serializable]
public class ScpSavesConfig
{
    public Resolucao resolucao;
    public LimiteFPS limiteFPS;
    public Qualidade qualidade;
    public bool modoJanela;
    public bool autoSave;
    public float geralVolume;
    public float musicaVolume;
    public float efeitosVolume;
}

public enum Qualidade
{
    Alta,
    Media,
    Baixa,
}

[Serializable]
public class Resolucao
{
    public int width;
    public int height;
}

[Serializable]
public class LimiteFPS
{
    public bool limitar;
    public int fps;
}
