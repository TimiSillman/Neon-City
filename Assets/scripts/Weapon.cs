
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject {

    new public string name = "Name";
    public GameObject mesh;
    public float damage;
    public float ammo;
    public float fireRate;
    public float projectileSpeed;
    public GameObject bullet;
    public bool isRapidFire;

}
