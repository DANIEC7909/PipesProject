using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pump :FluidTransfer
{


    [SerializeField] bool LogPaLA;
    public FluidTransfer source;
    private void Start()
    {
        //initsection
        fluidPerFlow = 1;
        maxFluidAmount = 700;
        //initsection
        presure = 100;
    }
    void Update()
    {
  
        if (LogPaLA)
        {
            Debug.Log("[PIPE |" + gameObject.name + "|]" + "Presure and Liquid Amount" + presure + " " + amountOfLiquid);
        }

    }
    void FixedUpdate()
    {
        if (source != null)
        { //do all if source !=null
            if (source.amountOfLiquid <= 0)
            {

            }
            else if (amountOfLiquid < maxFluidAmount)
            {
                    source.amountOfLiquid -= fluidPerFlow;
                    amountOfLiquid += fluidPerFlow;
                
            }
        }
    }
}

