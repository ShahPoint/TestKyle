using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Collections.Generic;

namespace KyleTanczos.TestKyle.Web.Models.App
{

    [XmlRoot(ElementName = "E01", Namespace = "http://www.nemsis.org")]
    public class E01
    {
        [XmlElement(ElementName = "E01_01", Namespace = "http://www.nemsis.org")]
        public string E01_01 { get; set; }
        [XmlElement(ElementName = "E01_02", Namespace = "http://www.nemsis.org")]
        public string E01_02 { get; set; }
        [XmlElement(ElementName = "E01_03", Namespace = "http://www.nemsis.org")]
        public string E01_03 { get; set; }
        [XmlElement(ElementName = "E01_04", Namespace = "http://www.nemsis.org")]
        public string E01_04 { get; set; }
    }

    [XmlRoot(ElementName = "E02", Namespace = "http://www.nemsis.org")]
    public class E02
    {
        [XmlElement(ElementName = "E02_01", Namespace = "http://www.nemsis.org")]
        public string E02_01 { get; set; }
        [XmlElement(ElementName = "E02_02", Namespace = "http://www.nemsis.org")]
        public string E02_02 { get; set; }
        [XmlElement(ElementName = "E02_03", Namespace = "http://www.nemsis.org")]
        public string E02_03 { get; set; }
        [XmlElement(ElementName = "E02_04", Namespace = "http://www.nemsis.org")]
        public string E02_04 { get; set; }
        [XmlElement(ElementName = "E02_05", Namespace = "http://www.nemsis.org")]
        public string E02_05 { get; set; }
        [XmlElement(ElementName = "E02_06", Namespace = "http://www.nemsis.org")]
        public List<string> E02_06 { get; set; }
        [XmlElement(ElementName = "E02_07", Namespace = "http://www.nemsis.org")]
        public List<string> E02_07 { get; set; }
        [XmlElement(ElementName = "E02_08", Namespace = "http://www.nemsis.org")]
        public List<string> E02_08 { get; set; }
        [XmlElement(ElementName = "E02_09", Namespace = "http://www.nemsis.org")]
        public List<string> E02_09 { get; set; }
        [XmlElement(ElementName = "E02_10", Namespace = "http://www.nemsis.org")]
        public List<string> E02_10 { get; set; }
        [XmlElement(ElementName = "E02_11", Namespace = "http://www.nemsis.org")]
        public string E02_11 { get; set; }
        [XmlElement(ElementName = "E02_12", Namespace = "http://www.nemsis.org")]
        public string E02_12 { get; set; }
        [XmlElement(ElementName = "E02_13", Namespace = "http://www.nemsis.org")]
        public string E02_13 { get; set; }
        [XmlElement(ElementName = "E02_14", Namespace = "http://www.nemsis.org")]
        public string E02_14 { get; set; }
        [XmlElement(ElementName = "E02_16", Namespace = "http://www.nemsis.org")]
        public string E02_16 { get; set; }
        [XmlElement(ElementName = "E02_17", Namespace = "http://www.nemsis.org")]
        public string E02_17 { get; set; }
        [XmlElement(ElementName = "E02_18", Namespace = "http://www.nemsis.org")]
        public string E02_18 { get; set; }
        [XmlElement(ElementName = "E02_19", Namespace = "http://www.nemsis.org")]
        public string E02_19 { get; set; }
        [XmlElement(ElementName = "E02_20", Namespace = "http://www.nemsis.org")]
        public string E02_20 { get; set; }
    }

    [XmlRoot(ElementName = "E03", Namespace = "http://www.nemsis.org")]
    public class E03
    {
        [XmlElement(ElementName = "E03_01", Namespace = "http://www.nemsis.org")]
        public string E03_01 { get; set; }
        [XmlElement(ElementName = "E03_02", Namespace = "http://www.nemsis.org")]
        public string E03_02 { get; set; }
    }

    [XmlRoot(ElementName = "E04", Namespace = "http://www.nemsis.org")]
    public class E04
    {
        [XmlElement(ElementName = "E04_01", Namespace = "http://www.nemsis.org")]
        public string E04_01 { get; set; }
        [XmlElement(ElementName = "E04_02", Namespace = "http://www.nemsis.org")]
        public string E04_02 { get; set; }
        [XmlElement(ElementName = "E04_03", Namespace = "http://www.nemsis.org")]
        public string E04_03 { get; set; }
    }

    [XmlRoot(ElementName = "E05_08", Namespace = "http://www.nemsis.org")]
    public class E05_08
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "E05", Namespace = "http://www.nemsis.org")]
    public class E05
    {
        [XmlElement(ElementName = "E05_01", Namespace = "http://www.nemsis.org")]
        public string E05_01 { get; set; }
        [XmlElement(ElementName = "E05_02", Namespace = "http://www.nemsis.org")]
        public string E05_02 { get; set; }
        [XmlElement(ElementName = "E05_03", Namespace = "http://www.nemsis.org")]
        public string E05_03 { get; set; }
        [XmlElement(ElementName = "E05_04", Namespace = "http://www.nemsis.org")]
        public string E05_04 { get; set; }
        [XmlElement(ElementName = "E05_05", Namespace = "http://www.nemsis.org")]
        public string E05_05 { get; set; }
        [XmlElement(ElementName = "E05_06", Namespace = "http://www.nemsis.org")]
        public string E05_06 { get; set; }
        [XmlElement(ElementName = "E05_08", Namespace = "http://www.nemsis.org")]
        public string E05_07 { get; set; }
        public string E05_08 { get; set; }
        [XmlElement(ElementName = "E05_11", Namespace = "http://www.nemsis.org")]
        public string E05_09 { get; set; }
        public string E05_10 { get; set; }
        public string E05_11 { get; set; }

        public string E05_12 { get; set; }
        public string E05_13 { get; set; }
    }

    [XmlRoot(ElementName = "E06_01_0", Namespace = "http://www.nemsis.org")]
    public class E06_01_0
    {
        [XmlElement(ElementName = "E06_01", Namespace = "http://www.nemsis.org")]
        public string E06_01 { get; set; }
        [XmlElement(ElementName = "E06_02", Namespace = "http://www.nemsis.org")]
        public string E06_02 { get; set; }
        [XmlElement(ElementName = "E06_03", Namespace = "http://www.nemsis.org")]
        public string E06_03 { get; set; }
    }

    [XmlRoot(ElementName = "E06_04_0", Namespace = "http://www.nemsis.org")]
    public class E06_04_0
    {
        [XmlElement(ElementName = "E06_04", Namespace = "http://www.nemsis.org")]
        public string E06_04 { get; set; }
        [XmlElement(ElementName = "E06_05", Namespace = "http://www.nemsis.org")]
        public string E06_05 { get; set; }
        [XmlElement(ElementName = "E06_07", Namespace = "http://www.nemsis.org")]
        public string E06_07 { get; set; }
        [XmlElement(ElementName = "E06_08", Namespace = "http://www.nemsis.org")]
        public string E06_08 { get; set; }
    }

    [XmlRoot(ElementName = "E06_14_0", Namespace = "http://www.nemsis.org")]
    public class E06_14_0
    {
        [XmlElement(ElementName = "E06_14", Namespace = "http://www.nemsis.org")]
        public string E06_14 { get; set; }
        [XmlElement(ElementName = "E06_15", Namespace = "http://www.nemsis.org")]
        public string E06_15 { get; set; }
    }

    [XmlRoot(ElementName = "E06", Namespace = "http://www.nemsis.org")]
    public class E06
    {
        [XmlElement(ElementName = "E06_01_0", Namespace = "http://www.nemsis.org")]
        public E06_01_0 E06_01_0 { get; set; }
        [XmlElement(ElementName = "E06_04_0", Namespace = "http://www.nemsis.org")]
        public E06_04_0 E06_04_0 { get; set; }
        [XmlElement(ElementName = "E06_06", Namespace = "http://www.nemsis.org")]
        public string E06_06 { get; set; }
        [XmlElement(ElementName = "E06_11", Namespace = "http://www.nemsis.org")]
        public string E06_11 { get; set; }
        [XmlElement(ElementName = "E06_12", Namespace = "http://www.nemsis.org")]
        public string E06_12 { get; set; }
        [XmlElement(ElementName = "E06_13", Namespace = "http://www.nemsis.org")]
        public string E06_13 { get; set; }
        [XmlElement(ElementName = "E06_14_0", Namespace = "http://www.nemsis.org")]
        public string E06_14 { get; set; }
        [XmlElement(ElementName = "E06_16", Namespace = "http://www.nemsis.org")]
        public string E06_16 { get; set; }
        [XmlElement(ElementName = "E06_17", Namespace = "http://www.nemsis.org")]
        public string E06_17 { get; set; }
    }

    [XmlRoot(ElementName = "E07_21_0", Namespace = "http://www.nemsis.org")]
    public class E07_21_0
    {
        [XmlElement(ElementName = "E07_21", Namespace = "http://www.nemsis.org")]
        public string E07_21 { get; set; }
        [XmlElement(ElementName = "E07_22", Namespace = "http://www.nemsis.org")]
        public string E07_22 { get; set; }
        [XmlElement(ElementName = "E07_23", Namespace = "http://www.nemsis.org")]
        public string E07_23 { get; set; }
        [XmlElement(ElementName = "E07_24", Namespace = "http://www.nemsis.org")]
        public string E07_24 { get; set; }
    }

    [XmlRoot(ElementName = "E07_18_0", Namespace = "http://www.nemsis.org")]
    public class E07_18_0
    {
        [XmlElement(ElementName = "E07_21_0", Namespace = "http://www.nemsis.org")]
        public E07_21_0 E07_21_0 { get; set; }
        [XmlElement(ElementName = "E07_25", Namespace = "http://www.nemsis.org")]
        public string E07_25 { get; set; }
        [XmlElement(ElementName = "E07_26", Namespace = "http://www.nemsis.org")]
        public string E07_26 { get; set; }
        [XmlElement(ElementName = "E07_18_01", Namespace = "http://www.nemsis.org")]
        public E07_18_01 E07_18_01 { get; set; }
    }

    [XmlRoot(ElementName = "E07_28_0", Namespace = "http://www.nemsis.org")]
    public class E07_28_0
    {
        [XmlElement(ElementName = "E07_28", Namespace = "http://www.nemsis.org")]
        public string E07_28 { get; set; }
        [XmlElement(ElementName = "E07_29", Namespace = "http://www.nemsis.org")]
        public string E07_29 { get; set; }
        [XmlElement(ElementName = "E07_30", Namespace = "http://www.nemsis.org")]
        public string E07_30 { get; set; }
        [XmlElement(ElementName = "E07_31", Namespace = "http://www.nemsis.org")]
        public string E07_31 { get; set; }
    }

    [XmlRoot(ElementName = "E07_27_0", Namespace = "http://www.nemsis.org")]
    public class E07_27_0
    {
        [XmlElement(ElementName = "E07_27", Namespace = "http://www.nemsis.org")]
        public string E07_27 { get; set; }
        [XmlElement(ElementName = "E07_28_0", Namespace = "http://www.nemsis.org")]
        public E07_28_0 E07_28_0 { get; set; }
    }

    [XmlRoot(ElementName = "E07_35_0", Namespace = "http://www.nemsis.org")]
    public class E07_35_0
    {
        [XmlElement(ElementName = "E07_35", Namespace = "http://www.nemsis.org")]
        public string E07_35 { get; set; }
        [XmlElement(ElementName = "E07_36", Namespace = "http://www.nemsis.org")]
        public string E07_36 { get; set; }
    }

    [XmlRoot(ElementName = "E07", Namespace = "http://www.nemsis.org")]
    public class E07
    {
        [XmlElement(ElementName = "E07_01", Namespace = "http://www.nemsis.org")]
        public string E07_01 { get; set; }
        [XmlElement(ElementName = "E07_15", Namespace = "http://www.nemsis.org")]
        public string E07_15 { get; set; }
        [XmlElement(ElementName = "E07_18_0", Namespace = "http://www.nemsis.org")]
        public E07_18_0 E07_18_0 { get; set; }
        [XmlElement(ElementName = "E07_27_0", Namespace = "http://www.nemsis.org")]
        public E07_27_0 E07_27_0 { get; set; }
        [XmlElement(ElementName = "E07_32", Namespace = "http://www.nemsis.org")]
        public string E07_32 { get; set; }
        [XmlElement(ElementName = "E07_34", Namespace = "http://www.nemsis.org")]
        public string E07_34 { get; set; }
        [XmlElement(ElementName = "E07_35_0", Namespace = "http://www.nemsis.org")]
        public string E07_35 { get; set; }
    }

    [XmlRoot(ElementName = "E08_11_0", Namespace = "http://www.nemsis.org")]
    public class E08_11_0
    {
        [XmlElement(ElementName = "E08_11", Namespace = "http://www.nemsis.org")]
        public string E08_11 { get; set; }
        [XmlElement(ElementName = "E08_12", Namespace = "http://www.nemsis.org")]
        public string E08_12 { get; set; }
        [XmlElement(ElementName = "E08_14", Namespace = "http://www.nemsis.org")]
        public string E08_14 { get; set; }
        [XmlElement(ElementName = "E08_15", Namespace = "http://www.nemsis.org")]
        public string E08_15 { get; set; }
    }

    [XmlRoot(ElementName = "E08", Namespace = "http://www.nemsis.org")]
    public class E08
    {
        [XmlElement(ElementName = "E08_02", Namespace = "http://www.nemsis.org")]
        public string E08_02 { get; set; }
        [XmlElement(ElementName = "E08_05", Namespace = "http://www.nemsis.org")]
        public string E08_05 { get; set; }
        [XmlElement(ElementName = "E08_06", Namespace = "http://www.nemsis.org")]
        public string E08_06 { get; set; }
        public string E08_07 { get; set; }

        public string E08_08 { get; set; }
        public string E08_09 { get; set; }

        public string E08_12 { get; set; }
        public string E08_13 { get; set; }
        public string E08_14 { get; set; }
        public string E08_15 { get; set; }

    }

    [XmlRoot(ElementName = "E09", Namespace = "http://www.nemsis.org")]
    public class E09
    {
        [XmlElement(ElementName = "E09_01", Namespace = "http://www.nemsis.org")]
        public List<string> E09_01 { get; set; }
        [XmlElement(ElementName = "E09_02", Namespace = "http://www.nemsis.org")]
        public string E09_02 { get; set; }
        [XmlElement(ElementName = "E09_03", Namespace = "http://www.nemsis.org")]
        public string E09_03 { get; set; }
        [XmlElement(ElementName = "E09_04", Namespace = "http://www.nemsis.org")]
        public string E09_04 { get; set; }
        [XmlElement(ElementName = "E09_05", Namespace = "http://www.nemsis.org")]
        public string E09_05 { get; set; }
        [XmlElement(ElementName = "E09_08", Namespace = "http://www.nemsis.org")]
        public string E09_08 { get; set; }
        [XmlElement(ElementName = "E09_11", Namespace = "http://www.nemsis.org")]
        public string E09_11 { get; set; }
        [XmlElement(ElementName = "E09_12", Namespace = "http://www.nemsis.org")]
        public string E09_12 { get; set; }
        [XmlElement(ElementName = "E09_13", Namespace = "http://www.nemsis.org")]
        public string E09_13 { get; set; }
        [XmlElement(ElementName = "E09_14", Namespace = "http://www.nemsis.org")]
        public List<string> E09_14 { get; set; }
        [XmlElement(ElementName = "E09_15", Namespace = "http://www.nemsis.org")]
        public string E09_15 { get; set; }
        [XmlElement(ElementName = "E09_16", Namespace = "http://www.nemsis.org")]
        public string E09_16 { get; set; }
    }

    [XmlRoot(ElementName = "E10", Namespace = "http://www.nemsis.org")]
    public class E10
    {
        [XmlElement(ElementName = "E10_01", Namespace = "http://www.nemsis.org")]
        public string E10_01 { get; set; }
        [XmlElement(ElementName = "E10_02", Namespace = "http://www.nemsis.org")]
        public string E10_02 { get; set; }
        [XmlElement(ElementName = "E10_03", Namespace = "http://www.nemsis.org")]
        public List<string> E10_03 { get; set; }
        [XmlElement(ElementName = "E10_10", Namespace = "http://www.nemsis.org")]
        public List<string> E10_04 { get; set; }
        public List<string> E10_08 { get; set; }
        public List<string> E10_09 { get; set; }

        public string E10_10 { get; set; }
    }

    [XmlRoot(ElementName = "E11", Namespace = "http://www.nemsis.org")]
    public class E11
    {
        [XmlElement(ElementName = "E11_01", Namespace = "http://www.nemsis.org")]
        public string E11_01 { get; set; }
        [XmlElement(ElementName = "E11_02", Namespace = "http://www.nemsis.org")]
        public string E11_02 { get; set; }
        [XmlElement(ElementName = "E11_03", Namespace = "http://www.nemsis.org")]
        public List<string> E11_03 { get; set; }
        [XmlElement(ElementName = "E11_07", Namespace = "http://www.nemsis.org")]
        public string E11_07 { get; set; }
        [XmlElement(ElementName = "E11_04", Namespace = "http://www.nemsis.org")]
        public string E11_04 { get; set; }
        [XmlElement(ElementName = "E11_05", Namespace = "http://www.nemsis.org")]
        public string E11_05 { get; set; }
        [XmlElement(ElementName = "E11_06", Namespace = "http://www.nemsis.org")]
        public string E11_06 { get; set; }
        [XmlElement(ElementName = "E11_08", Namespace = "http://www.nemsis.org")]
        public string E11_08 { get; set; }
        [XmlElement(ElementName = "E11_11", Namespace = "http://www.nemsis.org")]
        public string E11_11 { get; set; }
    }

    [XmlRoot(ElementName = "E12_4_0", Namespace = "http://www.nemsis.org")]
    public class E12_4_0
    {
        [XmlElement(ElementName = "E12_06", Namespace = "http://www.nemsis.org")]
        public string E12_06 { get; set; }
        [XmlElement(ElementName = "E12_04", Namespace = "http://www.nemsis.org")]
        public string E12_04 { get; set; }
    }

    [XmlRoot(ElementName = "E12_14_0", Namespace = "http://www.nemsis.org")]
    public class E12_14_0
    {
        [XmlElement(ElementName = "E12_14", Namespace = "http://www.nemsis.org")]
        public string E12_14 { get; set; }
    }

    [XmlRoot(ElementName = "E12", Namespace = "http://www.nemsis.org")]
    public class E12
    {
        [XmlElement(ElementName = "E12_01", Namespace = "http://www.nemsis.org")]
        public List<string> E12_01 { get; set; }
        [XmlElement(ElementName = "E12_4_0", Namespace = "http://www.nemsis.org")]
        public E12_4_0 E12_4_0 { get; set; }
        [XmlElement(ElementName = "E12_08", Namespace = "http://www.nemsis.org")]
        public List<string> E12_08 { get; set; }
        [XmlElement(ElementName = "E12_09", Namespace = "http://www.nemsis.org")]
        public List<string> E12_09 { get; set; }
        [XmlElement(ElementName = "E12_10", Namespace = "http://www.nemsis.org")]
        public List<string> E12_10 { get; set; }
        public List<string> E12_14 { get; set; }
        [XmlElement(ElementName = "E12_19", Namespace = "http://www.nemsis.org")]
        public List<string> E12_19 { get; set; }
        [XmlElement(ElementName = "E12_20", Namespace = "http://www.nemsis.org")]
        public string E12_20 { get; set; }
    }

    [XmlRoot(ElementName = "E13", Namespace = "http://www.nemsis.org")]
    public class E13
    {
        [XmlElement(ElementName = "E13_01", Namespace = "http://www.nemsis.org")]
        public string E13_01 { get; set; }
    }

    [XmlRoot(ElementName = "E14_04_0", Namespace = "http://www.nemsis.org")]
    public class E14_04_0
    {
        [XmlElement(ElementName = "E14_04", Namespace = "http://www.nemsis.org")]
        public string E14_04 { get; set; }
        [XmlElement(ElementName = "E14_05", Namespace = "http://www.nemsis.org")]
        public string E14_05 { get; set; }
        [XmlElement(ElementName = "E14_06", Namespace = "http://www.nemsis.org")]
        public string E14_06 { get; set; }
    }

    [XmlRoot(ElementName = "E14_13", Namespace = "http://www.nemsis.org")]
    public class E14_13
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "E14_15_0", Namespace = "http://www.nemsis.org")]
    public class E14_15_0
    {
        [XmlElement(ElementName = "E14_15", Namespace = "http://www.nemsis.org")]
        public string E14_15 { get; set; }
        [XmlElement(ElementName = "E14_16", Namespace = "http://www.nemsis.org")]
        public string E14_16 { get; set; }
        [XmlElement(ElementName = "E14_17", Namespace = "http://www.nemsis.org")]
        public string E14_17 { get; set; }
    }

    [XmlRoot(ElementName = "E14", Namespace = "http://www.nemsis.org")]
    public class E14
    {
        [XmlElement(ElementName = "E14_01", Namespace = "http://www.nemsis.org")]
        public string E14_01 { get; set; }
        [XmlElement(ElementName = "E14_02", Namespace = "http://www.nemsis.org")]
        public string E14_02 { get; set; }
        [XmlElement(ElementName = "E14_04_0", Namespace = "http://www.nemsis.org")]
        public string E14_04 { get; set; }
        public string E14_05 { get; set; }
        public string E14_06 { get; set; }

        [XmlElement(ElementName = "E14_07", Namespace = "http://www.nemsis.org")]
        public string E14_07 { get; set; }
        [XmlElement(ElementName = "E14_08", Namespace = "http://www.nemsis.org")]
        public string E14_08 { get; set; }
        [XmlElement(ElementName = "E14_09", Namespace = "http://www.nemsis.org")]
        public string E14_09 { get; set; }
        [XmlElement(ElementName = "E14_10", Namespace = "http://www.nemsis.org")]
        public string E14_10 { get; set; }
        [XmlElement(ElementName = "E14_11", Namespace = "http://www.nemsis.org")]
        public string E14_11 { get; set; }
        [XmlElement(ElementName = "E14_12", Namespace = "http://www.nemsis.org")]
        public string E14_12 { get; set; }
        [XmlElement(ElementName = "E14_13", Namespace = "http://www.nemsis.org")]
        public E14_13 E14_13 { get; set; }
        [XmlElement(ElementName = "E14_15_0", Namespace = "http://www.nemsis.org")]

        public string E14_15 { get; set; }
        public string E14_16 { get; set; }
        public string E14_17 { get; set; }
        public E14_15_0 E14_15_0 { get; set; }
        [XmlElement(ElementName = "E14_19", Namespace = "http://www.nemsis.org")]
        public string E14_19 { get; set; }
        [XmlElement(ElementName = "E14_22", Namespace = "http://www.nemsis.org")]
        public string E14_22 { get; set; }
        [XmlElement(ElementName = "E14_23", Namespace = "http://www.nemsis.org")]
        public string E14_23 { get; set; }
        [XmlElement(ElementName = "E14_27", Namespace = "http://www.nemsis.org")]
        public string E14_27 { get; set; }
        public string E14_28 { get; set; }
        [XmlElement(ElementName = "E14_03", Namespace = "http://www.nemsis.org")]
        public string E14_03 { get; set; }
        [XmlElement(ElementName = "E14_14", Namespace = "http://www.nemsis.org")]
        public string E14_14 { get; set; }
    }

    [XmlRoot(ElementName = "E15", Namespace = "http://www.nemsis.org")]
    public class E15
    {
        [XmlElement(ElementName = "E15_11", Namespace = "http://www.nemsis.org")]
        public string E15_11 { get; set; }
        [XmlElement(ElementName = "E15_05", Namespace = "http://www.nemsis.org")]
        public string E15_05 { get; set; }
        [XmlElement(ElementName = "E15_06", Namespace = "http://www.nemsis.org")]
        public string E15_06 { get; set; }
        [XmlElement(ElementName = "E15_09", Namespace = "http://www.nemsis.org")]
        public string E15_09 { get; set; }
        [XmlElement(ElementName = "E15_08", Namespace = "http://www.nemsis.org")]
        public string E15_08 { get; set; }
        [XmlElement(ElementName = "E15_03", Namespace = "http://www.nemsis.org")]
        public string E15_03 { get; set; }
        [XmlElement(ElementName = "E15_02", Namespace = "http://www.nemsis.org")]
        public List<string> E15_02 { get; set; }
        [XmlElement(ElementName = "E15_07", Namespace = "http://www.nemsis.org")]
        public string E15_07 { get; set; }
        [XmlElement(ElementName = "E15_10", Namespace = "http://www.nemsis.org")]
        public List<string> E15_10 { get; set; }
    }

    [XmlRoot(ElementName = "E16_00_0", Namespace = "http://www.nemsis.org")]
    public class E16_00_0
    {
        [XmlElement(ElementName = "E16_03", Namespace = "http://www.nemsis.org")]
        public string E16_03 { get; set; }
        [XmlElement(ElementName = "E16_04", Namespace = "http://www.nemsis.org")]
        public List<string> E16_04 { get; set; }
        [XmlElement(ElementName = "E16_07", Namespace = "http://www.nemsis.org")]
        public List<string> E16_07 { get; set; }
        [XmlElement(ElementName = "E16_21", Namespace = "http://www.nemsis.org")]
        public List<string> E16_21 { get; set; }
        [XmlElement(ElementName = "E16_22", Namespace = "http://www.nemsis.org")]
        public List<string> E16_22 { get; set; }
        [XmlElement(ElementName = "E16_24", Namespace = "http://www.nemsis.org")]
        public string E16_24 { get; set; }
        [XmlElement(ElementName = "E16_06", Namespace = "http://www.nemsis.org")]
        public string E16_06 { get; set; }
        [XmlElement(ElementName = "E16_08", Namespace = "http://www.nemsis.org")]
        public string E16_08 { get; set; }
        [XmlElement(ElementName = "E16_09", Namespace = "http://www.nemsis.org")]
        public string E16_09 { get; set; }
        [XmlElement(ElementName = "E16_10", Namespace = "http://www.nemsis.org")]
        public string E16_10 { get; set; }
        [XmlElement(ElementName = "E16_11", Namespace = "http://www.nemsis.org")]
        public string E16_11 { get; set; }
        [XmlElement(ElementName = "E16_12", Namespace = "http://www.nemsis.org")]
        public string E16_12 { get; set; }
        [XmlElement(ElementName = "E16_23", Namespace = "http://www.nemsis.org")]
        public List<string> E16_23 { get; set; }
        [XmlElement(ElementName = "E16_16", Namespace = "http://www.nemsis.org")]
        public string E16_16 { get; set; }
        [XmlElement(ElementName = "E16_05", Namespace = "http://www.nemsis.org")]
        public string E16_05 { get; set; }
        [XmlElement(ElementName = "E16_18", Namespace = "http://www.nemsis.org")]
        public string E16_18 { get; set; }
        [XmlElement(ElementName = "E16_20", Namespace = "http://www.nemsis.org")]
        public string E16_20 { get; set; }
        [XmlElement(ElementName = "E16_17", Namespace = "http://www.nemsis.org")]
        public string E16_17 { get; set; }
    }

    [XmlRoot(ElementName = "E16", Namespace = "http://www.nemsis.org")]
    public class E16
    {
        [XmlElement(ElementName = "E16_01", Namespace = "http://www.nemsis.org")]
        public string E16_01 { get; set; }
        [XmlElement(ElementName = "E16_00_0", Namespace = "http://www.nemsis.org")]
        public List<E16_00_0> E16_00_0 { get; set; }
    }

    [XmlRoot(ElementName = "E17", Namespace = "http://www.nemsis.org")]
    public class E17
    {
        [XmlElement(ElementName = "E17_01", Namespace = "http://www.nemsis.org")]
        public string E17_01 { get; set; }
    }

    [XmlRoot(ElementName = "E18", Namespace = "http://www.nemsis.org")]
    public class E18
    {
        [XmlElement(ElementName = "E18_03", Namespace = "http://www.nemsis.org")]
        public string E18_03 { get; set; }
        [XmlElement(ElementName = "E18_08", Namespace = "http://www.nemsis.org")]
        public string E18_08 { get; set; }
        [XmlElement(ElementName = "E18_02", Namespace = "http://www.nemsis.org")]
        public string E18_02 { get; set; }
        [XmlElement(ElementName = "E18_04", Namespace = "http://www.nemsis.org")]
        public string E18_04 { get; set; }
        [XmlElement(ElementName = "E18_05_0", Namespace = "http://www.nemsis.org")]
        public E18_05_0 E18_05_0 { get; set; }
        [XmlElement(ElementName = "E18_10", Namespace = "http://www.nemsis.org")]
        public string E18_10 { get; set; }
        [XmlElement(ElementName = "E18_11", Namespace = "http://www.nemsis.org")]
        public string E18_11 { get; set; }
        [XmlElement(ElementName = "E18_01", Namespace = "http://www.nemsis.org")]
        public string E18_01 { get; set; }
        [XmlElement(ElementName = "E18_09", Namespace = "http://www.nemsis.org")]
        public string E18_09 { get; set; }
    }

    [XmlRoot(ElementName = "E19_01_0", Namespace = "http://www.nemsis.org")]
    public class E19_01_0
    {
        [XmlElement(ElementName = "E19_01", Namespace = "http://www.nemsis.org")]
        public string E19_01 { get; set; }
        [XmlElement(ElementName = "E19_02", Namespace = "http://www.nemsis.org")]
        public string E19_02 { get; set; }
        [XmlElement(ElementName = "E19_03", Namespace = "http://www.nemsis.org")]
        public string E19_03 { get; set; }
        [XmlElement(ElementName = "E19_05", Namespace = "http://www.nemsis.org")]
        public string E19_05 { get; set; }
        [XmlElement(ElementName = "E19_06", Namespace = "http://www.nemsis.org")]
        public string E19_06 { get; set; }
        [XmlElement(ElementName = "E19_07", Namespace = "http://www.nemsis.org")]
        public string E19_07 { get; set; }
        [XmlElement(ElementName = "E19_08", Namespace = "http://www.nemsis.org")]
        public string E19_08 { get; set; }
        [XmlElement(ElementName = "E19_09", Namespace = "http://www.nemsis.org")]
        public string E19_09 { get; set; }
        [XmlElement(ElementName = "E19_10", Namespace = "http://www.nemsis.org")]
        public string E19_10 { get; set; }
        [XmlElement(ElementName = "E19_04", Namespace = "http://www.nemsis.org")]
        public string E19_04 { get; set; }
    }

    [XmlRoot(ElementName = "E19", Namespace = "http://www.nemsis.org")]
    public class E19
    {
        [XmlElement(ElementName = "E19_01_0", Namespace = "http://www.nemsis.org")]
        public List<E19_01_0> E19_01_0 { get; set; }
        [XmlElement(ElementName = "E19_12", Namespace = "http://www.nemsis.org")]
        public List<string> E19_12 { get; set; }
        [XmlElement(ElementName = "E19_13", Namespace = "http://www.nemsis.org")]
        public string E19_13 { get; set; }
        [XmlElement(ElementName = "E19_14", Namespace = "http://www.nemsis.org")]
        public string E19_14 { get; set; }
    }

    [XmlRoot(ElementName = "E20_03_0", Namespace = "http://www.nemsis.org")]
    public class E20_03_0
    {
        [XmlElement(ElementName = "E20_03", Namespace = "http://www.nemsis.org")]
        public string E20_03 { get; set; }
        [XmlElement(ElementName = "E20_04", Namespace = "http://www.nemsis.org")]
        public string E20_04 { get; set; }
        [XmlElement(ElementName = "E20_05", Namespace = "http://www.nemsis.org")]
        public string E20_05 { get; set; }
        [XmlElement(ElementName = "E20_07", Namespace = "http://www.nemsis.org")]
        public string E20_07 { get; set; }
    }

    [XmlRoot(ElementName = "E20_08", Namespace = "http://www.nemsis.org")]
    public class E20_08
    {
        [XmlAttribute(AttributeName = "Lat")]
        public string Lat { get; set; }
        [XmlAttribute(AttributeName = "Long")]
        public string Long { get; set; }
    }

    [XmlRoot(ElementName = "E20", Namespace = "http://www.nemsis.org")]
    public class E20
    {
        [XmlElement(ElementName = "E20_01", Namespace = "http://www.nemsis.org")]
        public string E20_01 { get; set; }
        [XmlElement(ElementName = "E20_02", Namespace = "http://www.nemsis.org")]
        public string E20_02 { get; set; }
        [XmlElement(ElementName = "E20_03_0", Namespace = "http://www.nemsis.org")]
        public E20_03_0 E20_03_0 { get; set; }
        [XmlElement(ElementName = "E20_06", Namespace = "http://www.nemsis.org")]
        public string E20_06 { get; set; }
        [XmlElement(ElementName = "E20_08", Namespace = "http://www.nemsis.org")]
        public E20_08 E20_08 { get; set; }
        [XmlElement(ElementName = "E20_09", Namespace = "http://www.nemsis.org")]
        public string E20_09 { get; set; }
        [XmlElement(ElementName = "E20_10", Namespace = "http://www.nemsis.org")]
        public string E20_10 { get; set; }
        [XmlElement(ElementName = "E20_14", Namespace = "http://www.nemsis.org")]
        public string E20_14 { get; set; }
        [XmlElement(ElementName = "E20_15", Namespace = "http://www.nemsis.org")]
        public string E20_15 { get; set; }
        [XmlElement(ElementName = "E20_16", Namespace = "http://www.nemsis.org")]
        public string E20_16 { get; set; }
        [XmlElement(ElementName = "E20_17", Namespace = "http://www.nemsis.org")]
        public string E20_17 { get; set; }
    }

    [XmlRoot(ElementName = "E22", Namespace = "http://www.nemsis.org")]
    public class E22
    {
        [XmlElement(ElementName = "E22_01", Namespace = "http://www.nemsis.org")]
        public string E22_01 { get; set; }
        [XmlElement(ElementName = "E22_02", Namespace = "http://www.nemsis.org")]
        public string E22_02 { get; set; }
        [XmlElement(ElementName = "E22_06", Namespace = "http://www.nemsis.org")]
        public string E22_06 { get; set; }
    }

    [XmlRoot(ElementName = "E23_09_0", Namespace = "http://www.nemsis.org")]
    public class E23_09_0
    {
        [XmlElement(ElementName = "E23_09", Namespace = "http://www.nemsis.org")]
        public string E23_09 { get; set; }
        [XmlElement(ElementName = "E23_11", Namespace = "http://www.nemsis.org")]
        public string E23_11 { get; set; }
    }

    [XmlRoot(ElementName = "E23", Namespace = "http://www.nemsis.org")]
    public class E23
    {
        [XmlElement(ElementName = "E23_03", Namespace = "http://www.nemsis.org")]
        public string E23_03 { get; set; }
        [XmlElement(ElementName = "E23_06", Namespace = "http://www.nemsis.org")]
        public string E23_06 { get; set; }
        [XmlElement(ElementName = "E23_09_0", Namespace = "http://www.nemsis.org")]
        public E23_09_0 E23_09_0 { get; set; }
        [XmlElement(ElementName = "E23_10", Namespace = "http://www.nemsis.org")]
        public string E23_10 { get; set; }
    }

    [XmlRoot(ElementName = "Record", Namespace = "http://www.nemsis.org")]
    public class Record
    {
        [XmlElement(ElementName = "E01", Namespace = "http://www.nemsis.org")]
        public E01 E01 { get; set; }
        [XmlElement(ElementName = "E02", Namespace = "http://www.nemsis.org")]
        public E02 E02 { get; set; }
        [XmlElement(ElementName = "E03", Namespace = "http://www.nemsis.org")]
        public E03 E03 { get; set; }
        [XmlElement(ElementName = "E04", Namespace = "http://www.nemsis.org")]
        public List<E04> E04 { get; set; }
        [XmlElement(ElementName = "E05", Namespace = "http://www.nemsis.org")]
        public E05 E05 { get; set; }
        [XmlElement(ElementName = "E06", Namespace = "http://www.nemsis.org")]
        public E06 E06 { get; set; }
        [XmlElement(ElementName = "E07", Namespace = "http://www.nemsis.org")]
        public E07 E07 { get; set; }
        [XmlElement(ElementName = "E08", Namespace = "http://www.nemsis.org")]
        public E08 E08 { get; set; }
        [XmlElement(ElementName = "E09", Namespace = "http://www.nemsis.org")]
        public E09 E09 { get; set; }
        [XmlElement(ElementName = "E10", Namespace = "http://www.nemsis.org")]
        public E10 E10 { get; set; }
        [XmlElement(ElementName = "E11", Namespace = "http://www.nemsis.org")]
        public E11 E11 { get; set; }
        [XmlElement(ElementName = "E12", Namespace = "http://www.nemsis.org")]
        public E12 E12 { get; set; }
        [XmlElement(ElementName = "E13", Namespace = "http://www.nemsis.org")]
        public E13 E13 { get; set; }
        [XmlElement(ElementName = "E14", Namespace = "http://www.nemsis.org")]
        public List<E14> E14 { get; set; }
        [XmlElement(ElementName = "E15", Namespace = "http://www.nemsis.org")]
        public E15 E15 { get; set; }
        [XmlElement(ElementName = "E16", Namespace = "http://www.nemsis.org")]
        public E16 E16 { get; set; }
        [XmlElement(ElementName = "E17", Namespace = "http://www.nemsis.org")]
        public List<E17> E17 { get; set; }
        [XmlElement(ElementName = "E18", Namespace = "http://www.nemsis.org")]
        public List<E18> E18 { get; set; }
        [XmlElement(ElementName = "E19", Namespace = "http://www.nemsis.org")]
        public E19 E19 { get; set; }
        [XmlElement(ElementName = "E20", Namespace = "http://www.nemsis.org")]
        public E20 E20 { get; set; }
        [XmlElement(ElementName = "E22", Namespace = "http://www.nemsis.org")]
        public E22 E22 { get; set; }
        [XmlElement(ElementName = "E23", Namespace = "http://www.nemsis.org")]
        public E23 E23 { get; set; }
    }

    [XmlRoot(ElementName = "E18_05_0", Namespace = "http://www.nemsis.org")]
    public class E18_05_0
    {
        [XmlElement(ElementName = "E18_05", Namespace = "http://www.nemsis.org")]
        public string E18_05 { get; set; }
        [XmlElement(ElementName = "E18_06", Namespace = "http://www.nemsis.org")]
        public string E18_06 { get; set; }
    }

    [XmlRoot(ElementName = "E08_10", Namespace = "http://www.nemsis.org")]
    public class E08_10
    {
        [XmlAttribute(AttributeName = "Lat")]
        public string Lat { get; set; }
        [XmlAttribute(AttributeName = "Long")]
        public string Long { get; set; }
    }

    [XmlRoot(ElementName = "E05_07", Namespace = "http://www.nemsis.org")]
    public class E05_07
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "E05_09", Namespace = "http://www.nemsis.org")]
    public class E05_09
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "E05_10", Namespace = "http://www.nemsis.org")]
    public class E05_10
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "E07_18_01", Namespace = "http://www.nemsis.org")]
    public class E07_18_01
    {
        [XmlElement(ElementName = "E07_18", Namespace = "http://www.nemsis.org")]
        public string E07_18 { get; set; }
        [XmlElement(ElementName = "E07_19", Namespace = "http://www.nemsis.org")]
        public string E07_19 { get; set; }
    }

    [XmlRoot(ElementName = "Header", Namespace = "http://www.nemsis.org")]
    public class Header
    {
        [XmlElement(ElementName = "D01_01", Namespace = "http://www.nemsis.org")]
        public string D01_01 { get; set; }
        [XmlElement(ElementName = "D01_03", Namespace = "http://www.nemsis.org")]
        public string D01_03 { get; set; }
        [XmlElement(ElementName = "D01_04", Namespace = "http://www.nemsis.org")]
        public string D01_04 { get; set; }
        [XmlElement(ElementName = "D01_07", Namespace = "http://www.nemsis.org")]
        public string D01_07 { get; set; }
        [XmlElement(ElementName = "D01_08", Namespace = "http://www.nemsis.org")]
        public string D01_08 { get; set; }
        [XmlElement(ElementName = "D01_09", Namespace = "http://www.nemsis.org")]
        public string D01_09 { get; set; }
        [XmlElement(ElementName = "D01_21", Namespace = "http://www.nemsis.org")]
        public string D01_21 { get; set; }
        [XmlElement(ElementName = "D02_07", Namespace = "http://www.nemsis.org")]
        public string D02_07 { get; set; }
        [XmlElement(ElementName = "Record", Namespace = "http://www.nemsis.org")]
        public List<Record> Record { get; set; }
    }

    [XmlRoot(ElementName = "EMSDataSet", Namespace = "http://www.nemsis.org")]
    public class EMSDataSet
    {
        [XmlElement(ElementName = "Header", Namespace = "http://www.nemsis.org")]
        public Header Header { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string SchemaLocation { get; set; }
    }
}