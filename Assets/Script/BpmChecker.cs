using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;

public class BpmChecker
{
    /// <summary></summary>
    const int _minBpm = 32;
    const int _maxBpm = 400;

    /// <summary>標本化周波数（サンプリング周波数）</summary>
    const int _baseFrequency = 44100;

    /// <summary>基本的なチャンネル数</summary>
    const int _baseCh = 2;

    /// <summary>サンプルデータの基本分割サイズ()</summary>
    const int _baseSplitSize = 2205;

    public struct BpmMatchData
    {
        public int bpm;
        public float match;
    }

    static BpmMatchData[] _bpmMatchDatas = new BpmMatchData[_maxBpm - _minBpm + 1];

    /// <summary>
    ///BPMを調べるための関数
    /// </summary>
    /// <param name="clip">音源</param>
    /// <param name="frequency">音源の周波数</param>
    /// <returns></returns>
    public static float AnalyzeBpm(AudioClip clip ,int frequency)
    {

        ////与えられた音源の周波数を確認
        //int _frequency = clip.frequency;
        //Debug.Log("周波数 :" + _frequency);

        //与えられた音源のオーディオチャンネル数
        int _channel = clip.channels;
        //Debug.Log("オーディオチャンネル数 :" + _channel);


        //フレームサイズを調べる（与えられた音源の周波数/ベースの周波数)*(オーディオ クリップのチャンネル数/ベースのチャンネル)*スプリットサンプルサイズ
        int _flrameSize = Mathf.FloorToInt(((float)frequency / (float)_baseFrequency) * ((float)_channel / (float)_baseCh) * (float)_baseSplitSize);
        //Debug.Log("フレームサイズ :" + _flrameSize);

        //audioclip からすべてのサンプル データを取得する
        var _allSanples = new float[clip.samples * _channel];
        clip.GetData(_allSanples, 0);

        //
        var _allVolume = CreateVolume(_allSanples, frequency, _channel, _flrameSize);

        //ボリュームー配列からBPMを検索
        int _bpm = K(_allVolume, frequency, _flrameSize);
        //Debug.Log("BPM :" + _bpm );

        if(_bpm > 200)
        {
            _bpm = _bpm / 2;
            //Debug.Log("変更後のBPM " + _bpm);
        }


        return _bpm;
    }

    /// <summary>
    /// ボリューム配列の作成コード
    /// </summary>
    /// <param name="allSunples">すべてのサンプルデータ</param>
    /// <param name="frequency">音源の周波数</param>
    /// <param name="channel">チャンネル数</param>
    /// <param name="flrameSize">フレームサイズ</param>
    /// <returns></returns>
    private static float[] CreateVolume(float[] allSunples, int frequency, int channel, int flrameSize)
    {
        //切り上げ
        var volume = new float[Mathf.CeilToInt((float)allSunples.Length / (float)flrameSize)];
        //Debug.Log(volume.Length);
        int Index = 0;

        for (int i = 0; i < allSunples.Length; i += flrameSize)
        {
            float sum = 0f;
            for (int frameIndex = i; frameIndex < i + flrameSize; frameIndex++)
            {

                if (allSunples.Length <= frameIndex)
                {
                    break;
                }

                //絶対値
                float adsValue = Mathf.Abs(allSunples[frameIndex]);
                if (adsValue > 1f)
                {
                    continue;
                }

                sum += (adsValue * adsValue);
            }

            //平方根　（絶対値/フレームサイズ）
            volume[Index] = MathF.Sqrt(sum / flrameSize);
            Index++;
        }

        float maxVolume = volume.Max();
        for (int i = 0; i < volume.Length; i++)
        {
            volume[i] = volume[i] / maxVolume;
        }

        return volume;
    }

    /// <summary>
    /// 適正なBPMの値を出すための関数
    /// </summary>
    /// <param name="allVolume">ボリューム配列</param>
    /// <param name="frequency">音源の周波数</param>
    /// <param name="frameSize">フレームサイズ</param>
    /// <returns></returns>
    private static int K(float[] allVolume, int frequency, int frameSize)
    {
        var list = new List<float>();

        for (int i = 1; i < allVolume.Length; i++)
        {
            list.Add(Mathf.Max(allVolume[i] - allVolume[i - 1], 0f));
        }

        int _index = 0;
        float splitFrequency = (float)frequency / (float)frameSize; //分割周波数
        for (int bpm = _minBpm; bpm <= _maxBpm; bpm++)
        {
            float s = 0f;
            float c = 0f;
            float bps = (float)bpm / 60f;

            if(list.Count > 0)
            {
                for(int i =0; i < list.Count; i++)
                {
                    s += (list[i] * Mathf.Cos(i * 2f * Mathf.PI * bps / splitFrequency));
                    c += (list[i] * Mathf.Sin(i * 2f * Mathf.PI * bps / splitFrequency));
                }

                s *= (1f / (float)list.Count);
                c *= (1f / (float)list.Count);
            }

            float match = Mathf.Sqrt((s * s) + (c * c));

            _bpmMatchDatas[_index].bpm = bpm;
            _bpmMatchDatas[_index].match = match;
            _index++;
        }

        int matchIndex = Array.FindIndex(_bpmMatchDatas, x => x.match == _bpmMatchDatas.Max(y => y.match));

        return _bpmMatchDatas[matchIndex].bpm;
    }
}
