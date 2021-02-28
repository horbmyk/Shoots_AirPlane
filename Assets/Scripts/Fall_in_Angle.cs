using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Fall_in_Angle : MonoBehaviour
{
    public GameObject cannon_hend;
    public GameObject bullet;
    public GameObject bullet_traectory;
    public GameObject ShootElement;
    public GameObject airplane;
    public Text Angle_lebel;
    public Text V_0_lebel;
    float V_0;
    float StartTime_airplane = 0;
    float X_0;
    float Y_0;
    float Z;
    float X_traectory;
    float Y_traectory;
    float X_airplane;
    float Y_airplane;
    float Z_airplane;
    int Ghange_Y_airplane;
    List<GameObject> PoolBullet;
    bool Go;
    public GameObject boom;
    public Slider slider_angle;
    public Slider slider_V_0;
    public float speed = 1;
    public ShootBomb shootBomb;
    List<ShootBomb> PoolShootBombs;

    void Start()
    {
        Change_V_0(slider_V_0.value);
        Set_Angle(slider_angle.value);
        PoolBullet = new List<GameObject>();
        X_0 = ShootElement.transform.position.x;
        Y_0 = ShootElement.transform.position.y;
        Z = ShootElement.transform.position.z;
        for (int i = 0; i < 50; i++)
        {
            PoolBullet.Add(Instantiate(bullet_traectory));
        }

        airplane.transform.position = new Vector3(ShootElement.transform.position.x + 25f,
            ShootElement.transform.position.y + 8f, ShootElement.transform.position.z);

        X_airplane = ShootElement.transform.position.x + 25f;
        Y_airplane = ShootElement.transform.position.y + 8f;
        Z_airplane = ShootElement.transform.position.z;
        boom.SetActive(false);
        Ghange_Y_airplane = UnityEngine.Random.Range(-4, 4);
        speed = UnityEngine.Random.Range(1, 4);
        PoolShootBombs = new List<ShootBomb>();
    }


    void Update()
    {
        PrintTraectory();
        SetPositionAirPlane();
        Set_Angle_Arrow();
        Set_Speed_V_0_Arrow();
        PrintLabel();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void PrintTraectory()
    {
        for (int i = 0; i < 50; i++)
        {
            X_traectory = X_0 + V_0 * Mathf.Cos((360 - cannon_hend.transform.localEulerAngles.x) * Mathf.PI / 180) * (i * 0.1f);
            Y_traectory = Y_0 + V_0 * Mathf.Sin((360 - cannon_hend.transform.localEulerAngles.x) * Mathf.PI / 180) * (i * 0.1f)
                - 9.81f * ((i * 0.1f) * (i * 0.1f)) / 2;
            PoolBullet[i].transform.position = new Vector3(X_traectory, Y_traectory, Z);
        }
        if (Go)
        {
            for (int i = 0; i < PoolShootBombs.Count; i++)
            {

                PoolShootBombs[i].Bullet.transform.position =
                new Vector3(X_0 + PoolShootBombs[i].V_0 * Mathf.Cos((PoolShootBombs[i].Angle) * Mathf.PI / 180) * (Time.time - PoolShootBombs[i].StartTime),
                Y_0 + PoolShootBombs[i].V_0 * Mathf.Sin((PoolShootBombs[i].Angle) * Mathf.PI / 180) * (Time.time - PoolShootBombs[i].StartTime)
                - 9.81f * ((Time.time - PoolShootBombs[i].StartTime) * (Time.time - PoolShootBombs[i].StartTime)) / 2,
                Z);
                if (PoolShootBombs[i].Bullet.transform.position.y >= airplane.transform.position.y - 1 &&
                    PoolShootBombs[i].Bullet.transform.position.y <= airplane.transform.position.y + 1)
                {
                    if (PoolShootBombs[i].Bullet.transform.position.x >= airplane.transform.position.x - 1.7f
                        && PoolShootBombs[i].Bullet.transform.position.x <= airplane.transform.position.x + 1.7f)
                    {
                        Destroy(PoolShootBombs[i].Bullet);
                        PoolShootBombs.Remove(PoolShootBombs[i]);
                        boom.SetActive(true);
                        boom.transform.position = airplane.transform.position;
                        StartTime_airplane = Time.time;
                        Ghange_Y_airplane = UnityEngine.Random.Range(-4, 4);
                        speed = UnityEngine.Random.Range(1, 4);
                        break;
                    }
                }
                if (PoolShootBombs[i].Bullet.transform.position.y <= 0)
                {
                    Destroy(PoolShootBombs[i].Bullet);
                    PoolShootBombs.Remove(PoolShootBombs[i]);

                }


            }
        }
    }
    public void Shoot()
    {
        shootBomb = new ShootBomb(Instantiate(bullet), (360 - cannon_hend.transform.localEulerAngles.x), V_0, Time.time);
        PoolShootBombs.Add(shootBomb);
        Go = true;
    }
    void Set_Angle_Arrow()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {

            slider_angle.value -= 0.2f;

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            slider_angle.value += 0.2f;
        }
    }
    void Set_Speed_V_0_Arrow()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            slider_V_0.value -= 0.2f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            slider_V_0.value += 0.2f;
        }
    }

    public void Set_Angle(float _x)
    {
        cannon_hend.transform.localEulerAngles = new Vector3(_x, 0, 0);
    }
    public void Change_V_0(float _V_0)
    {
        V_0 = _V_0;
    }
    void SetPositionAirPlane()
    {
        if (airplane.transform.position.x < ShootElement.transform.position.x - 3)
        {
            StartTime_airplane = Time.time;
            Ghange_Y_airplane = UnityEngine.Random.Range(-4, 4);
            speed = UnityEngine.Random.Range(1, 4);

        }
        airplane.transform.position = new Vector3(X_airplane - ((Time.time - StartTime_airplane) * speed), Y_airplane + Ghange_Y_airplane, Z_airplane);
        if (Time.time - StartTime_airplane >= 1.5f)
        {
            boom.SetActive(false);
        }

    }

    void PrintLabel()
    {
        int angle = 360 - (int)cannon_hend.transform.localEulerAngles.x;

        if (angle == 360)
        {
            Angle_lebel.text = "Angle=" + Math.Abs(slider_angle.value);
        }
        else
        {
            Angle_lebel.text = "Angle= " + angle;
        }

        V_0_lebel.text = "V(0)= " + (int)V_0;
    }

}
public class ShootBomb
{
    public GameObject Bullet;
    public float Angle;
    public float V_0;
    public float StartTime;
    public ShootBomb(GameObject _Bullet, float _Angle, float _V_0, float _StartTime)
    {
        Bullet = _Bullet;
        Angle = _Angle;
        V_0 = _V_0;
        StartTime = _StartTime;

    }






}