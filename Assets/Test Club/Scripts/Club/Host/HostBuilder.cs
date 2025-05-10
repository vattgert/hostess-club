using System;
using System.Collections.Generic;
using UnityEngine;


public interface IHostBuilder
{
    IHostBuilder SetHostData(Host host);
    IHostBuilder SetActive(bool active);
    GameObject Build();
}

public class HostBuilder : IHostBuilder
{
    private GameObject _host;

    private void SetGameObjectName(Host host)
    {
        _host.name = "Host " + host.Name;
    }

    public HostBuilder(GameObject prefab)
    {
        _host = GameObject.Instantiate(prefab);
    }

    public IHostBuilder SetHostData(Host host)
    {
        _host.GetComponent<HostBehavior>().Initialize(host);
        SetGameObjectName(host);
        return this;
    }

    public IHostBuilder SetActive(bool active)
    {
        _host.SetActive(active);
        return this;
    }

    public GameObject Build()
    {
        return _host;
    }
}
