using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetParameterCS
{
    /// <summary>
    /// データテーブルから必要な情報のみ読み取って
    /// 与えられたフレーム（単体・複数）の情報を
    /// 輝度からLive2d用パラメータの値に変換する
    /// </summary>
    class ConvBrighttoLive2D
    {
        private struct Idproperty
        {
            public int min;
            public int max;
        }
        private List<Idproperty> idproperties;

        public ConvBrighttoLive2D( DataTable editTable)
        {
            SetProperty(editTable);
        }

        private void SetProperty(DataTable editTable)
        {
            idproperties = new List<Idproperty>();

            for (int j = 0; j < editTable.Rows.Count; j++)
            {
                Idproperty idp = new Idproperty
                {
                    min = (int)editTable.Rows[j][1],
                    max = (int)editTable.Rows[j][2]
                };
                idproperties.Add(idp);
            }
        }

        /// <summary>
        /// 全フレーム変換する
        /// </summary>
        /// <param name="inputframes"></param>
        public List<List<float>> Comvert(List<List<float>> inputframes)
        {
            List<List<float>> outputframes = new List<List<float>>();
            foreach (var inputframe in inputframes)
            {
                outputframes.Add(Comvert(inputframe));
            }
            return outputframes;
        }
        /// <summary>
        /// フレーム一枚ずつ値を変換する
        /// </summary>
        /// <param name="inputframe"></param>
        /// <returns></returns>
        public List<float> Comvert(List<float> inputframe)
        {
            List<float> outputframe = new List<float>();
            for (int i = 0; i < inputframe.Count; i++)
            {
                outputframe.Add(ComvertValue(inputframe[i], idproperties[i]));
            }
            return outputframe;
        }

        /// <summary>
        /// Live2Dの最小値と最大値セットから輝度を変換する
        /// </summary>
        /// <param name="value"></param>
        /// <param name="idproperty"></param>
        /// <returns></returns>
        private float ComvertValue(float value,Idproperty idproperty)
        {
            return ComvertValue(value, idproperty.min, idproperty.max);
        }
        public float ComvertValue(float value,int min,int max)
        {
            return (value * (max - min)) + min;
        }
    }
}
