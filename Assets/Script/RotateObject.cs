using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Pengaturan Rotasi")]
    [Tooltip("Tentukan kecepatan putar di sumbu X, Y, atau Z. Nilai minus untuk putar balik.")]
    public Vector3 rotationSpeed = new Vector3(0f, 200f, 0f);

    void Update()
    {

        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}