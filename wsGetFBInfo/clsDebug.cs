﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace wsGetFBInfo
{
    public class clsDebug
    {
        static void Main(string[] args)
        {
            Service sv = new Service();
            sv.TmrUpdateHistory_BDL_Elapsed(null, null);

            //string[] a = "2445703^^^,,^11761^1575^0^1^-1^48^2023,9,31,02,11,08^^72,5,42^98,10,58^^0^1^images/20170424193242.png^images/1575/1gq207hy3wt.png^0!8%,27%;1,17%,5%^20%,8%,22%;1,18%^18%,8%,14%,18%^14%,13%,9%,18%^14%,13%,12%,26%;1^22%;1,27%;1,22%;1,13%!9%,23%;1,16%,5%^22%;1,6%,23%;1,17%^18%,6%,16%,22%^14%,19%,9%,13%^15%,20%,12%,24%;1^20%,20%,21%,17%!553,0,4,,-1,95,,0,1,2743,,!3,1,11761,21,2,^5,1,11761,21,1,^6,1,11761,20,1,^9,1,1575,21,1,^12,1,1575,21,1,^13,1,1575,20,1,^14,1,1575,21,1,^16,1,1575,20,1,^17,1,1575,21,1,^19,1,1575,21,1,^20,1,1575,20,1,^23,1,1575,20,2,^24,1,1575,21,2,^25,1,1575,20,2,^28,1,11761,21,3,^30,1,1575,20,3,^33,1,1575,21,4,^34,1,1575,20,4,^37,1,1575,21,4,^38,1,1575,20,4,^40,1,1575,21,5,^42,1,1575,21,5,^43,1,1575,20,5,^46,1,11761,21,5,^48,1,11761,21,6,^50,1,11761,20,6,^52,1,1575,21,6,^54,1,1575,21,6,^56,1,11761,21,7,^57,1,11761,20,7,^59,1,11761,20,7,^60,1,11761,21,8,^62,1,11761,20,8,^64,1,11761,20,8,^66,1,1575,20,8,^67,1,1575,21,8,^68,1,1575,20,8,^70,1,1575,20,9,^73,1,1575,20,10,^76,1,11761,21,11,^77,1,11761,20,11,^80,1,1575,21,11,^82,1,1575,21,12,^83,1,1575,20,12,^85,1,11761,21,12,^88,1,11761,21,13,^89,1,11761,20,13,^90,1,11761,21,13,^92,1,1575,21,13,^94,1,11761,21,14,^96,1,1575,21,14,^97,1,1575,20,14,^98,1,1575,21,14,^100,1,1575,21,14,^101,1,1575,20,14,^102,1,1575,21,14,^104,1,1575,21,15,^105,1,1575,20,15,^108,1,1575,21,16,^110,1,1575,21,16,^112,1,1575,21,16,^113,1,1575,20,16,^114,1,1575,21,16,^115,1,1575,20,16,^117,1,1575,21,17,^118,1,1575,20,17,^121,1,1575,20,18,^124,1,11761,21,18,^126,1,1575,21,18,^127,1,1575,20,19,^129,1,1575,20,20,^130,1,1575,21,20,^131,1,1575,20,20,^132,1,1575,21,20,^133,1,1575,20,20,^134,1,1575,21,21,^135,1,1575,20,21,^136,1,1575,21,21,^137,1,1575,20,21,^138,1,1575,21,21,^142,1,11761,21,25,^144,1,1575,21,25,^145,1,1575,20,25,^148,1,11761,21,26,^152,1,11761,21,27,^155,1,11761,20,27,^158,1,1575,21,28,^159,1,1575,20,28,^164,1,1575,21,31,^165,1,1575,20,31,^167,1,11761,21,32,^170,1,1575,20,33,^172,1,11761,21,33,^173,1,11761,20,33,^176,1,1575,21,34,^177,1,1575,20,34,^179,1,1575,20,34,^182,1,11761,21,36,^183,1,11761,20,36,^186,1,1575,20,37,^187,1,1575,21,37,^188,1,1575,20,37,^189,1,1575,21,37,^191,1,1575,21,38,^193,1,1575,21,38,^194,1,1575,20,38,^195,1,1575,21,38,^196,1,1575,20,38,^197,1,1575,21,38,^198,1,1575,20,38,^201,1,11761,21,39,^202,1,11761,20,39,^204,1,11761,21,39,^205,1,11761,20,39,^207,1,11761,20,40,^209,1,11761,20,41,^212,1,11761,21,41,^214,1,11761,20,42,^216,1,11761,20,42,^217,1,11761,21,42,^219,1,11761,21,42,^222,1,11761,20,43,^223,1,11761,21,44,^226,1,11761,21,44,^229,1,11761,20,44,^231,1,11761,20,45,^232,1,11761,21,45,^233,1,11761,20,45,^236,1,11761,21,45,^237,1,11761,20,45,^240,1,1575,21,46,^241,1,1575,20,46,^242,1,1575,21,46,^244,1,1575,21,47,^246,1,1575,21,47,^247,1,1575,20,47,^249,1,11761,21,47,^251,1,1575,20,47,^253,1,11761,21,48,^258,1,1575,21,49,^259,1,1575,20,49,^264,1,1575,21,46,^266,1,1575,21,46,^267,1,1575,20,46,^269,1,1575,21,46,^270,1,1575,20,46,^275,1,11761,21,47,^277,1,11761,21,47,^279,1,1575,20,47,^282,1,1575,20,48,^283,1,1575,21,48,^284,1,1575,20,48,^285,1,1575,21,48,^287,1,11761,21,48,^288,1,11761,20,48,^290,1,1575,21,49,^291,1,1575,20,49,^293,1,11761,21,50,^296,1,11761,21,50,^298,1,1575,21,50,^299,1,1575,20,51,^301,1,1575,21,51,^303,1,1575,21,51,^305,1,1575,21,51,^307,1,1575,21,51,^309,1,1575,20,51,^311,1,1575,20,51,^318,1,11761,21,52,^320,1,11761,21,53,^321,1,11761,20,53,^325,1,1575,21,53,^328,1,1575,21,54,^330,1,1575,21,54,^332,1,1575,21,54,^334,1,1575,21,54,^336,1,11761,21,55,^340,1,1575,21,55,^341,1,1575,20,56,^344,1,1575,20,56,^346,1,11761,21,57,^347,1,11761,20,57,^350,1,1575,21,58,^353,1,1575,20,59,^355,1,11761,21,60,^357,1,1575,21,60,^358,1,1575,20,60,^360,1,11761,21,60,^362,1,1575,21,60,^363,1,1575,20,60,^365,1,1575,20,61,^369,1,11761,21,62,^372,1,1575,21,62,^373,1,1575,20,62,^375,1,11761,21,62,^376,1,11761,20,62,^377,1,11761,21,62,^379,1,1575,21,62,^383,1,11761,21,63,^385,1,11761,20,64,^387,1,1575,21,64,^388,1,1575,20,64,^390,1,11761,21,64,^392,1,1575,21,65,^393,1,1575,20,65,^397,1,1575,21,66,^399,1,1575,21,66,^400,1,1575,20,66,^402,1,11761,20,67,^408,1,1575,21,69,^410,1,1575,21,70,^413,1,11761,21,70,^414,1,11761,20,70,^418,1,1575,21,71,^420,1,11761,21,71,^421,1,11761,20,71,^423,1,11761,20,71,^427,1,1575,20,73,^431,1,1575,20,73,^434,1,11761,21,74,^436,1,11761,21,74,^437,1,11761,20,74,^438,1,11761,21,74,^440,1,1575,21,74,^444,1,1575,21,75,^445,1,1575,20,75,^449,1,11761,21,76,^450,1,11761,20,76,^454,1,1575,21,77,^455,1,1575,20,77,^457,1,11761,21,77,^459,1,1575,21,77,^461,1,1575,21,78,^462,1,1575,20,78,^464,1,11761,21,78,^467,1,11761,21,79,^469,1,11761,20,80,^470,1,11761,21,80,^472,1,11761,20,80,^474,1,1575,21,81,^476,1,1575,20,82,^479,1,1575,21,82,^480,1,1575,20,82,^482,1,11761,21,83,^484,1,11761,21,83,^486,1,1575,21,83,^487,1,1575,20,83,^490,1,1575,20,84,^492,1,11761,21,85,^495,1,11761,21,85,^496,1,11761,20,85,^499,1,1575,20,86,^501,1,11761,21,86,^502,1,11761,20,86,^504,1,11761,21,86,^506,1,1575,21,87,^507,1,1575,20,87,^511,1,11761,21,87,^513,1,11761,20,88,^514,1,11761,21,88,^515,1,11761,20,88,^517,1,1575,21,88,^519,1,1575,21,89,^521,1,1575,20,89,^525,1,11761,21,90,^527,1,1575,21,90,^529,1,11761,21,90,^530,1,11761,20,90,^532,1,11761,21,90,^534,1,1575,21,90,^535,1,1575,20,90,^537,1,11761,21,91,^538,1,11761,20,91,^542,1,1575,21,94,^543,1,1575,20,94,^545,1,11761,21,94,^547,1,1575,21,94,^549,1,11761,21,94,^551,1,1575,21,95,^1,2,1575,1,10,0^2,2,1575,1,11,0^3,2,1575,1,19,0^4,2,11761,1,42,0^5,2,1575,1,52,0^6,2,1575,1,57,0^7,2,1575,1,62,0^8,2,11761,1,72,0^9,2,11761,1,81,0^10,2,1575,1,85,0^1,3,1575,1,32,0^1,4,1575,3,28,0^2,4,1575,3,44,0^3,4,11761,3,45,5^4,4,1575,3,53,0^5,4,11761,3,59,0^6,4,1575,3,79,0^!".Split('^');

        }
    }
}
