using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeRemaster : FluidTransfer 
{
  
    [SerializeField] bool LogPaLA;
    public FluidTransfer source;
    private void Start()
    {
        //init
        fluidPerFlow = 1;
        maxFluidAmount = 500;
        //init
    }
    void Update()
    {
        if (source != null)
            presure = source.presure - 20;


        if (LogPaLA)//temporary
        {
            Debug.Log("[PIPE |" +gameObject.name+ "|]" + "Presure and Liquid Amount" + presure + " " + amountOfLiquid);
        }

    }
    void FixedUpdate()
    {
        if (source != null)
        { //do all if source !=null
            if (source.amountOfLiquid <= 0)
            {

            }
            else if(amountOfLiquid <maxFluidAmount)
            {//if liquid amount > 0
                if (Mathf.Abs(source.presure) > Mathf.Abs(presure))//allow flow when s.presure is grater than this.presure
                {
                    source.amountOfLiquid -= fluidPerFlow;
                    amountOfLiquid += fluidPerFlow;
                }
            }
        }
    }
}
