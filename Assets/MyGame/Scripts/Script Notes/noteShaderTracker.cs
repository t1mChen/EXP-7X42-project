using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sourced from workshop9 solution and edited from it

public class ShaderValueChanger : MonoBehaviour
{
    // controll variables for phong shader
    [Range(0, 1)] [SerializeField] private float ka = 1;
    [Range(0, 1)] [SerializeField] private float kd = 1;
    [Range(0, 1)] [SerializeField] private float ks = 1;
    [Range(0, 1)] [SerializeField] private float fAtt = 1;
    [Range(1, 100)] [SerializeField] private float specN = 5;
    private Color color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        // update the value of lighting information
        Material mat = this.GetComponent<MeshRenderer>().material;
        mat.SetFloat("_Ka", ka);
        mat.SetFloat("_Kd", kd);
        mat.SetFloat("_Ks", ks);
        mat.SetFloat("_fAtt", fAtt);
        mat.SetFloat("_specN", specN);
        // select a color to represent the player
        mat.SetColor("_PointLightColor", color);
        // attach the position of player towards the phong model
        Vector3 pos = GameObject.Find("Player").transform.position + Vector3.up;
        mat.SetVector("_PointLightPosition", pos);

    }
}