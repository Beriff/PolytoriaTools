using System;
using System.Collections.Generic;
using System.Text;

namespace PolytoriaTools
{
    class UserAppearance
    {
        public UserAppearance(string hash, int head_color, int torso_color, int leftarm_color, 
            int rightarm_color, int leftleg_color, int rightleg_color, int hatid1, int hatid2, 
            int hatid3, int faceid, int toolid, int shirtid, int pantsid, int headid, int tshirtid)
        {
            this.hash = hash;
            this.head_color = head_color;
            this.torso_color = torso_color;
            this.leftarm_color = leftarm_color;
            this.rightarm_color = rightarm_color;
            this.leftleg_color = leftleg_color;
            this.rightleg_color = rightleg_color;
            this.hatid1 = hatid1;
            this.hatid2 = hatid2;
            this.hatid3 = hatid3;
            this.faceid = faceid;
            this.toolid = toolid;
            this.shirtid = shirtid;
            this.pantsid = pantsid;
            this.headid = headid;
            this.tshirtid = tshirtid;
        }

        public Asset GetHat1()
        {
            return Asset.GetAssetById(hatid1);
        }

        public Asset GetHat2()
        {
            return Asset.GetAssetById(hatid2);
        }

        public Asset GetHat3()
        {
            return Asset.GetAssetById(hatid3);
        }

        public Asset GetFace()
        {
            return Asset.GetAssetById(faceid);
        }

        public Asset GetTool()
        {
            return Asset.GetAssetById(toolid);
        }

        public Asset GetShirt()
        {
            return Asset.GetAssetById(shirtid);
        }

        public Asset GetPants()
        {
            return Asset.GetAssetById(pantsid);
        }

        [Obsolete("Head Assets are not implemented yet", true)]
        public Asset GetHead()
        {
            return Asset.GetAssetById(headid);
        }

        public Asset GetTShirt()
        {
            return Asset.GetAssetById(tshirtid);
        }

        public string hash { get; private set; }
        public int head_color { get; private set; }
        public int torso_color { get; private set; }
        public int leftarm_color { get; private set; }
        public int rightarm_color { get; private set; }
        public int leftleg_color { get; private set; }
        public int rightleg_color { get; private set; }
        public int hatid1 { get; private set; }
        public int hatid2 { get; private set; }
        public int hatid3 { get; private set; }
        public int faceid { get; private set; }
        public int toolid { get; private set; }
        public int shirtid { get; private set; }
        public int pantsid { get; private set; }
        public int headid { get; private set; }
        public int tshirtid { get; private set; }

    }
}
