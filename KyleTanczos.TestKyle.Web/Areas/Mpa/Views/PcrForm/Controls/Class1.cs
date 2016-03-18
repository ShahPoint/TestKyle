using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{

    public class E01
    {

        public string E01_01 { get; set; }
   
        public string E01_02 { get; set; }

        public string E01_03 { get; set; }

        public string E01_04 { get; set; }
    }


    public class E02_15
    {

        public string Lat { get; set; }
   
        public string Long { get; set; }
    }


    public class E02
    {

        public string E02_01 { get; set; }

        public string E02_02 { get; set; }
  
        public string E02_03 { get; set; }

        public string E02_04 { get; set; }
        public string E02_05 { get; set; }
        public string E02_06 { get; set; }
        public string E02_07 { get; set; }
        public string E02_08 { get; set; }
        public string E02_09 { get; set; }
        public string E02_10 { get; set; }
        public string E02_11 { get; set; }
        public string E02_12 { get; set; }
        public string E02_13 { get; set; }
        public string E02_14 { get; set; }
        public E02_15 E02_15 { get; set; }
        public string E02_16 { get; set; }
        public string E02_17 { get; set; }
        public string E02_18 { get; set; }
        public string E02_19 { get; set; }
        public string E02_20 { get; set; }
    }

    public class E03
    {
        public string E03_01 { get; set; }
        public string E03_02 { get; set; }
        public string E03_03 { get; set; }
    }

    public class E04
    {
        public string E04_01 { get; set; }
        public string E04_02 { get; set; }
        public string E04_03 { get; set; }
    }

    public class E05
    {
        public string E05_01 { get; set; }
        public string E05_02 { get; set; }
        public string E05_03 { get; set; }
        public string E05_04 { get; set; }
        public string E05_05 { get; set; }
        public string E05_06 { get; set; }
        public string E05_07 { get; set; }
        public string E05_08 { get; set; }
        public string E05_09 { get; set; }
        public string E05_10 { get; set; }
        public string E05_11 { get; set; }
        public string E05_12 { get; set; }
        public string E05_13 { get; set; }
    }

    public class E06_01_0
    {
        public string E06_01 { get; set; }
        public string E06_02 { get; set; }
        public string E06_03 { get; set; }
    }

    public class E06_04_0
    {
        public string E06_04 { get; set; }
        public string E06_05 { get; set; }
        public string E06_07 { get; set; }
        public string E06_08 { get; set; }
    }

    public class E06_14_0
    {
        public string E06_14 { get; set; }
        public string E06_15 { get; set; }
    }

    public class E06_19_0
    {
        public string E06_18 { get; set; }
        public string E06_19 { get; set; }
    }

    public class E06
    {
        public E06_01_0 E06_01_0 { get; set; }
        public E06_04_0 E06_04_0 { get; set; }
        public string E06_06 { get; set; }
        public string E06_09 { get; set; }
        public string E06_10 { get; set; }
        public string E06_11 { get; set; }
        public string E06_12 { get; set; }
        public string E06_13 { get; set; }
        public E06_14_0 E06_14_0 { get; set; }
        public string E06_16 { get; set; }
        public string E06_17 { get; set; }
        public E06_19_0 E06_19_0 { get; set; }
    }

    public class E07_05_0
    {
        public string E07_05 { get; set; }
        public string E07_06 { get; set; }
        public string E07_07 { get; set; }
        public string E07_08 { get; set; }
    }

    public class E07_11_0
    {
        public string E07_11 { get; set; }
        public string E07_12 { get; set; }
        public string E07_13 { get; set; }
    }

    public class E07_03_0
    {
        public string E07_03 { get; set; }
        public string E07_04 { get; set; }
        public E07_05_0 E07_05_0 { get; set; }
        public string E07_09 { get; set; }
        public string E07_10 { get; set; }
        public E07_11_0 E07_11_0 { get; set; }
        public string E07_14 { get; set; }
    }

    public class E07_18_01
    {
    
        public string E07_18 { get; set; }

        public string E07_19 { get; set; }

        public string E07_20 { get; set; }
    }


    public class E07_21_0
    {
        public string E07_21 { get; set; }
        public string E07_22 { get; set; }
        public string E07_23 { get; set; }
        public string E07_24 { get; set; }
    }

    public class E07_18_0
    {
        public E07_18_01 E07_18_01 { get; set; }
        public E07_21_0 E07_21_0 { get; set; }
        public string E07_25 { get; set; }
        public string E07_26 { get; set; }
    }

    public class E07_28_0
    {
        public string E07_28 { get; set; }
        public string E07_29 { get; set; }
        public string E07_30 { get; set; }
        public string E07_31 { get; set; }
    }

    public class E07_27_0
    {
        public string E07_27 { get; set; }
        public E07_28_0 E07_28_0 { get; set; }
    }

    public class E07_35_0
    {
        public string E07_35 { get; set; }
        public string E07_36 { get; set; }
    }

    public class E07
    {
        public string E07_01 { get; set; }
        public string E07_02 { get; set; }
        public List<E07_03_0> E07_03_0 { get; set; }
        public string E07_15 { get; set; }
        public string E07_16 { get; set; }
        public string E07_17 { get; set; }
        public E07_18_0 E07_18_0 { get; set; }
        public E07_27_0 E07_27_0 { get; set; }
        public string E07_32 { get; set; }
        public string E07_33 { get; set; }
        public string E07_34 { get; set; }
        public E07_35_0 E07_35_0 { get; set; }
        public List<string> E07_37 { get; set; }
    }

    public class E08_10
    {
        public string Lat { get; set; }
        public string Long { get; set; }
    }

    public class E08_11_0
    {
        public string E08_11 { get; set; }
        public string E08_12 { get; set; }
        public string E08_14 { get; set; }
        public string E08_15 { get; set; }
    }

    
    public class E08
    {
        public List<string> E08_01 { get; set; }
        public List<string> E08_02 { get; set; }
        public string E08_03 { get; set; }
        public string E08_04 { get; set; }
        public string E08_05 { get; set; }
        public string E08_06 { get; set; }
        public string E08_07 { get; set; }
        public string E08_08 { get; set; }
        public string E08_09 { get; set; }
        public E08_10 E08_10 { get; set; }
        public E08_11_0 E08_11_0 { get; set; }
        public string E08_13 { get; set; }
    }

    public class E09_06_0
    {
        public string E09_06 { get; set; }
        public string E09_07 { get; set; }
    }

    public class E09_09_0
    {
        public string E09_09 { get; set; }
        public string E09_10 { get; set; }
    }

    public class E09
    {
        public string E09_01 { get; set; }
        public string E09_02 { get; set; }
        public string E09_03 { get; set; }
        public string E09_04 { get; set; }
        public string E09_05 { get; set; }

        public E09_06_0 E09_06_0 { get; set; }
        
        public string E09_08 { get; set; }
     
        public E09_09_0 E09_09_0 { get; set; }
        
        public string E09_11 { get; set; }
        
        public string E09_12 { get; set; }
        
        public string E09_13 { get; set; }
        
        public string E09_14 { get; set; }
        
        public string E09_15 { get; set; }
        
        public string E09_16 { get; set; }
    }

    
    public class E10_06_0
    {
        
        public string E10_06 { get; set; }
        
        public string E10_07 { get; set; }
    }

    
    public class E10
    {
        
        public string E10_01 { get; set; }
        
        public string E10_02 { get; set; }
        
        public List<string> E10_03 { get; set; }
        
        public List<string> E10_04 { get; set; }
        
        public List<string> E10_05 { get; set; }
        
        public E10_06_0 E10_06_0 { get; set; }
        
        public List<string> E10_08 { get; set; }
        
        public List<string> E10_09 { get; set; }
        
        public string E10_10 { get; set; }
    }

    
    public class E11
    {
        
        public string E11_01 { get; set; }
        
        public string E11_02 { get; set; }
        
        public string E11_03 { get; set; }
        
        public string E11_04 { get; set; }
        
        public string E11_05 { get; set; }
        
        public string E11_06 { get; set; }
        
        public string E11_07 { get; set; }
        
        public string E11_08 { get; set; }
        
        public string E11_09 { get; set; }
        
        public string E11_10 { get; set; }
        
        public List<string> E11_11 { get; set; }
    }

    
    public class E12_4_0
    {
        
        public string E12_06 { get; set; }
        
        public string E12_04 { get; set; }
        
        public string E12_05 { get; set; }
    }

    
    public class E12_12_0
    {
        
        public string E12_12 { get; set; }
        
        public string E12_13 { get; set; }
    }

    
    public class E12_15_0
    {
        
        public string E12_15 { get; set; }
        
        public string E12_16 { get; set; }
    }

    
    public class E12_14_0
    {
        
        public string E12_14 { get; set; }
        
        public E12_15_0 E12_15_0 { get; set; }
        
        public string E12_17 { get; set; }
    }

    
    public class E12
    {
        
        public string E12_01 { get; set; }
        
        public string E12_02 { get; set; }
        
        public string E12_03 { get; set; }
        
        public E12_4_0 E12_4_0 { get; set; }
        
        public List<string> E12_07 { get; set; }
        
        public List<string> E12_08 { get; set; }
        
        public List<string> E12_09 { get; set; }
        
        public List<string> E12_10 { get; set; }
        
        public string E12_11 { get; set; }
        
        public List<E12_12_0> E12_12_0 { get; set; }
        
        public List<E12_14_0> E12_14_0 { get; set; }
        
        public string E12_18 { get; set; }
        
        public string E12_19 { get; set; }
        
        public string E12_20 { get; set; }
    }

    
    public class E13
    {
        
        public string E13_01 { get; set; }
    }

    
    public class E14_04_0
    {
        
        public string E14_04 { get; set; }
        
        public string E14_05 { get; set; }
        
        public string E14_06 { get; set; }
    }

    
    public class E14_15_0
    {
        
        public string E14_15 { get; set; }
        
        public string E14_16 { get; set; }
        
        public string E14_17 { get; set; }
        
        public string E14_18 { get; set; }
    }

    
    public class E14_20_0
    {
        
        public string E14_20 { get; set; }
        
        public string E14_21 { get; set; }
    }

    
    public class E14
    {
        
        public string E14_01 { get; set; }
        
        public string E14_02 { get; set; }
        
        public List<string> E14_03 { get; set; }
        
        public E14_04_0 E14_04_0 { get; set; }
        
        public string E14_07 { get; set; }
        
        public string E14_08 { get; set; }
        
        public string E14_09 { get; set; }
        
        public string E14_10 { get; set; }
        
        public string E14_11 { get; set; }
        
        public string E14_12 { get; set; }
        
        public string E14_13 { get; set; }
        
        public string E14_14 { get; set; }
        
        public E14_15_0 E14_15_0 { get; set; }
        
        public string E14_19 { get; set; }
        
        public E14_20_0 E14_20_0 { get; set; }
        
        public string E14_22 { get; set; }
        
        public string E14_23 { get; set; }
        
        public string E14_24 { get; set; }
        
        public string E14_25 { get; set; }
        
        public string E14_26 { get; set; }
        
        public string E14_27 { get; set; }
        
        public string E14_28 { get; set; }
    }

    
    public class E15
    {
        
        public List<string> E15_01 { get; set; }
        
        public List<string> E15_02 { get; set; }
        
        public List<string> E15_03 { get; set; }
        
        public List<string> E15_04 { get; set; }
        
        public List<string> E15_05 { get; set; }
        
        public List<string> E15_06 { get; set; }
        
        public List<string> E15_07 { get; set; }
        
        public List<string> E15_08 { get; set; }
        
        public List<string> E15_09 { get; set; }
        
        public List<string> E15_10 { get; set; }
        
        public List<string> E15_11 { get; set; }
    }

    
    public class E16_00_0
    {
        
        public string E16_03 { get; set; }
        
        public List<string> E16_04 { get; set; }
        
        public List<string> E16_05 { get; set; }
        
        public List<string> E16_06 { get; set; }
        
        public List<string> E16_07 { get; set; }
        
        public List<string> E16_08 { get; set; }
        
        public List<string> E16_09 { get; set; }
        
        public List<string> E16_10 { get; set; }
        
        public List<string> E16_11 { get; set; }
        
        public List<string> E16_12 { get; set; }
        
        public List<string> E16_13 { get; set; }
        
        public List<string> E16_14 { get; set; }
        
        public List<string> E16_15 { get; set; }
        
        public List<string> E16_16 { get; set; }
        
        public List<string> E16_17 { get; set; }
        
        public List<string> E16_18 { get; set; }
        
        public List<string> E16_19 { get; set; }
        
        public List<string> E16_20 { get; set; }
        
        public List<string> E16_21 { get; set; }
        
        public List<string> E16_22 { get; set; }
        
        public List<string> E16_23 { get; set; }
        
        public List<string> E16_24 { get; set; }
    }

    
    public class E16
    {
        
        public string E16_01 { get; set; }
        
        public string E16_02 { get; set; }
        
        public List<E16_00_0> E16_00_0 { get; set; }
    }

    
    public class E17
    {
        
        public string E17_01 { get; set; }
    }

    
    public class E18_05_0
    {
        
        public string E18_05 { get; set; }
        
        public string E18_06 { get; set; }
    }

    
    public class E18
    {
        
        public string E18_01 { get; set; }
        
        public string E18_02 { get; set; }
        
        public string E18_03 { get; set; }
        
        public string E18_04 { get; set; }
        
        public E18_05_0 E18_05_0 { get; set; }
        
        public string E18_07 { get; set; }
        
        public string E18_08 { get; set; }
        
        public string E18_09 { get; set; }
        
        public string E18_10 { get; set; }
        
        public string E18_11 { get; set; }
    }

    
    public class E19_01_0
    {
        
        public string E19_01 { get; set; }
        
        public string E19_02 { get; set; }
        
        public string E19_03 { get; set; }
        
        public string E19_04 { get; set; }
        
        public string E19_05 { get; set; }
        
        public string E19_06 { get; set; }
        
        public string E19_07 { get; set; }
        
        public string E19_08 { get; set; }
        
        public string E19_09 { get; set; }
        
        public string E19_10 { get; set; }
        
        public string E19_11 { get; set; }
    }

    
    public class E19
    {
        
        public E19_01_0 E19_01_0 { get; set; }
        
        public List<string> E19_12 { get; set; }
        
        public List<string> E19_13 { get; set; }
        
        public List<string> E19_14 { get; set; }
    }

    
    public class Address
    {

        public string E20_03 { get; set; }

        public string E20_04 { get; set; }

        public string E20_05 { get; set; }

        public string E20_07 { get; set; }
    }

    
 
        

    

    
    public class E20
    {
        
        public string E20_01 { get; set; }
        
        public string E20_02 { get; set; }
        
        public Address E20_Address { get; set; }
        
        public string E20_06 { get; set; }

        public string E20_08_Lat { get; set; }

        public string E20_08_Long { get; set; }

        public string E20_09 { get; set; }
        
        public string E20_10 { get; set; }
        
        public string E20_11 { get; set; }
        
        public string E20_12 { get; set; }
        
        public string E20_13 { get; set; }
        
        public string E20_14 { get; set; }
        
        public string E20_15 { get; set; }
        
        public string E20_16 { get; set; }
        
        public string E20_17 { get; set; }
    }

    
    public class E21_03_0
    {
        
        public string E21_03 { get; set; }
        
        public string E21_04 { get; set; }
    }

    
    public class E21_18_0
    {
        
        public string E21_18 { get; set; }
        
        public string E21_19 { get; set; }
    }

    //Devices
    public class E21
    {
        
        public string E21_01 { get; set; }
        
        public string E21_02 { get; set; }
        
        public E21_03_0 E21_03_0 { get; set; }
        
        public string E21_05 { get; set; }
        
        public string E21_06 { get; set; }
        
        public string E21_07 { get; set; }
        
        public string E21_08 { get; set; }
        
        public string E21_09 { get; set; }
        
        public string E21_10 { get; set; }
        
        public string E21_11 { get; set; }
        
        public string E21_12 { get; set; }
        
        public string E21_13 { get; set; }
        
        public string E21_14 { get; set; }
        
        public string E21_15 { get; set; }
        
        public string E21_16 { get; set; }
        
        public string E21_17 { get; set; }
        
        public E21_18_0 E21_18_0 { get; set; }
        
        public string E21_20 { get; set; }
    }

    
    public class MergeAAA
    {
        
        public string E22_01 { get; set; }
        
        public string E22_02 { get; set; }
        
        public string E22_03 { get; set; }
        
        public string E22_04 { get; set; }
        
        public string E22_05 { get; set; }
        
        public string E22_06 { get; set; }
 
        
        public string E23_01 { get; set; }
        
        public List<string> E23_02 { get; set; }
        
        public List<string> E23_03 { get; set; }
        
        public List<string> E23_04 { get; set; }
        
        public string E23_05 { get; set; }
        
        public List<string> E23_06 { get; set; }
        
        public List<string> E23_07 { get; set; }
        
        public string E23_08 { get; set; }

        public string E23_09 { get; set; }

        public string E23_10 { get; set; }

        public string E23_11 { get; set; }
    }

    
    public class Record
    {
        
        public E01 E01 { get; set; }
        
        public E02 E02 { get; set; }
        
        public E03 E03 { get; set; }
        
        public List<E04> E04 { get; set; }
        
        public E05 E05 { get; set; }
        
        public E06 E06 { get; set; }
        
        public E07 E07 { get; set; }
        
        public E08 E08 { get; set; }
        
        public E09 E09 { get; set; }
        
        public E10 E10 { get; set; }
        
        public E11 E11 { get; set; }
        
        public E12 E12 { get; set; }
        
        public E13 E13 { get; set; }
        
        public List<E14> E14 { get; set; }
        
        public E15 E15 { get; set; }
        
        public E16 E16 { get; set; }
        
        public List<E17> E17 { get; set; }
        
        public E18 E18 { get; set; }
        
        public E19 E19 { get; set; }
        
        public E20 E20 { get; set; }
        
        public List<E21> E21 { get; set; }
        
    }

    
    public class Header
    {
        
        public string D01_01 { get; set; }
        
        public string D01_03 { get; set; }
        
        public string D01_04 { get; set; }
        
        public string D01_07 { get; set; }
        
        public string D01_08 { get; set; }
        
        public string D01_09 { get; set; }
        
        public string D01_21 { get; set; }
        
        public string D02_07 { get; set; }
        
        public Record Record { get; set; }
    }

    
    public class EMSDataSet
    {
        
        public Header Header { get; set; }
        
        public string Xmlns { get; set; }
        
        public string Xsi { get; set; }
        
        public string SchemaLocation { get; set; }
    }

}
