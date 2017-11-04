using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour {
    GameObject player;
    Stats _stats;

    public void Save()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _stats = player.GetComponent<Stats>();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData wololol = new PlayerData();
        wololol.saveHealth = _stats.health;
        wololol.saveDamage = _stats.damage;
        wololol.saveExp = _stats.exp;
        wololol.saveStat = _stats.stat;
        wololol.saveVida = _stats.VidaActual;
        wololol.saveLvl = _stats.Levels;
        wololol.saveIndex = SceneManager.GetActiveScene().buildIndex;
        bf.Serialize(file, wololol);
        file.Close();
        Debug.Log("Save");
    }
    [Serializable]
    class PlayerData
    {
        public float saveHealth;
        public float saveExp;
        public float saveDamage;
        public int saveStat;
        public float saveVida;
        public int saveLvl;
        public int saveIndex;
    }
    public void Load()
    {
        if ((File.Exists(Application.persistentDataPath + "/playerInfo.dat")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData wololol = (PlayerData)bf.Deserialize(file);
            Variables.Ghealth = wololol.saveHealth;
            Variables.Gdamage = wololol.saveDamage;
            Variables.Gexp = wololol.saveExp;
            Variables.Gstat = wololol.saveStat;
            Variables.Gvida = wololol.saveVida;
            Variables.Glvl = wololol.saveLvl;
            Variables.Gindex = wololol.saveIndex;
            SceneManager.LoadScene(Variables.Gindex);
            file.Close();
        }
        else
            Debug.Log("No hay save");
    }
}
