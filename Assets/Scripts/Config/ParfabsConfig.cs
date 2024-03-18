using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParfabsConfig", menuName = "Create ParfabsConfig")]
public class ParfabsConfig : ScriptableObject
{
    public GameObject snakePrafab;
    public GameObject wallPrafabs;
    public GameObject foodPrafabs;
}
