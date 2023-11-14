using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };  // ����, ���Ÿ�
    public enum characterType { Player, Troll, Boss };
    public Type type;
    public int damage;
    public float rate;  // ���ݼӵ�
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    string temp;
    private void Start()
    { 
        temp = LayerMask.LayerToName(this.gameObject.layer);
        if (this.gameObject.transform.root.name == "Troll")
            temp = this.gameObject.transform.root.name;
        else if(this.gameObject.transform.root.name == "Ÿ��ź")
            temp = this.gameObject.transform.root.name;
    }
    public void Use()
    {
        if (type == Type.Melee)
        {
            switch (temp)
            {
                case "Player":
                    damage = GameObject.Find(temp).GetComponent<Player>().AttackDamage;
                    rate = GameObject.Find(temp).GetComponent<Player>().Rate;
                    break;
                case "Troll":
                    damage = GameObject.Find(temp).GetComponentInChildren<Troll>().AttackDamage;
                    rate = GameObject.Find(temp).GetComponentInChildren<Troll>().Rate;
                    break;
                case "Ÿ��ź":
                    damage = GameObject.Find(temp).GetComponent<Boss>().AttackDamage;
                    rate = GameObject.Find(temp).GetComponent<Boss>().Rate;
                    break;
                default:
                    break;
            }

            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        if(trailEffect != null)
            trailEffect.enabled = true;

        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.2f);
        if (trailEffect != null)
            trailEffect.enabled = false;
    }
}

