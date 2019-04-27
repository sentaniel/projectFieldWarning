﻿/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using UnityEngine;

public class DestructibleStructure : MonoBehaviour
{
    [SerializeField]
    private GameObject _intactModel = null;
    [SerializeField]
    private GameObject _ruinsModel = null;

    // Use this for initialization
    private void Start()
    {
        if (_intactModel == null)
            throw new System.Exception("Structure has no undamaged model!");
        if (_ruinsModel == null)
            throw new System.Exception("Structure has no damaged model!");
    }

    // Update is called once per frame
    private void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        _intactModel.SetActive(false);
        _ruinsModel.SetActive(true);
    }
}
