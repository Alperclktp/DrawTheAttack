﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/VFX", fileName = "VFX", order = 2)]
public class VFXSO : ScriptableObject
{
    public VFXType type;

    public GameObject prefab;
}