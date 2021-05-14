﻿////////////////////////////////////////////////////////////////////
//                            _ooOoo_                             //
//                           o8888888o                            //
//                           88" . "88                            //
//                           (| ^_^ |)                            //
//                           O\  =  /O                            //
//                        ____/`---'\____                         //
//                      .'  \\|     |//  `.                       //
//                     /  \\|||  :  |||//  \                      //
//                    /  _||||| -:- |||||-  \                     //
//                    |   | \\\  -  /// |   |                     //
//                    | \_|  ''\---/''  |   |                     //
//                    \  .-\__  `-`  ___/-. /                     //
//                  ___`. .'  /--.--\  `. . ___                   //
//                ."" '<  `.___\_<|>_/___.'  >'"".                //
//              | | :  `- \`.;`\ _ /`;.`/ - ` : | |               //
//              \  \ `-.   \_ __\ /__ _/   .-` /  /               //
//        ========`-.____`-.___\_____/___.-`____.-'========       //
//                             `=---='                            //
//        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^      //
//            佛祖保佑       无BUG        不修改                   //
////////////////////////////////////////////////////////////////////
/*
 * 描述：
 * 作者：slicol
*/

namespace SGF.Network.FSPLite.Client
{
    public class FSPFrameController
    {
        //缓冲控制
        private int m_NewestFrameId;
        private int m_BuffSize = 0;
        private bool m_IsInBuffing = false;
        private int m_ClientFrameRateMultiple = 2;

        //加速控制
        private bool m_EnableSpeedUp = true;
        private int m_DefaultSpeed = 1;
        private bool m_IsInSpeedUp = false;
        
        //自动缓冲
        private bool m_EnableAutoBuff = true;
        private int m_AutoBuffCnt = 0;
        private int m_AutoBuffInterval = 15;
  
        public void Start(FSPParam param)
        {
            SetParam(param);
        }

        public void Close()
        {
        }

        public bool IsInBuffing { get { return m_IsInBuffing; } }
        public bool IsInSpeedUp { get { return m_IsInSpeedUp; } }
        public int FrameBufferSize { get { return m_BuffSize; } set { m_BuffSize = value; } }
        public int NewestFrameId { get { return m_NewestFrameId;} }

        public void SetParam(FSPParam param)
        {
            m_ClientFrameRateMultiple = param.clientFrameRateMultiple;
            m_BuffSize = param.frameBufferSize;
            m_EnableSpeedUp = param.enableSpeedUp;
            m_DefaultSpeed = param.defaultSpeed;
            m_EnableAutoBuff = param.enableAutoBuffer;
        }

        


        public void AddFrameId(int frameId)
        {
            m_NewestFrameId = frameId;
        }


        public int GetFrameSpeed(int curFrameId)
        {
            int speed = 0;

            int newFrameNum = m_NewestFrameId - curFrameId;

            //是否正在缓冲中
            if (!m_IsInBuffing)
            {
                //没有在缓冲中

                if (newFrameNum == 0)
                {
                    //需要缓冲一下
                    m_IsInBuffing = true;
                    m_AutoBuffCnt = m_AutoBuffInterval;
                }
                else
                {
                    //因为即将播去这么多帧
                    newFrameNum -= m_DefaultSpeed;

                    //剩下的可加速的帧数
                    int speedUpFrameNum = newFrameNum - m_BuffSize;
                    if (speedUpFrameNum >= m_ClientFrameRateMultiple)
                    {
                        //可以加速
                        if (m_EnableSpeedUp)
                        {
                            speed = speedUpFrameNum > 100 ? 8 : 2;
                        }
                        else
                        {
                            speed = m_DefaultSpeed;
                        }
                    }
                    else
                    {
                        //还达不到可加速的帧数
                        speed = m_DefaultSpeed;

                        //主动缓冲，当帧数过少时，
                        //与其每一帧都卡，不如先卡久一点，然后就不卡了

                        if (m_EnableAutoBuff)
                        {
                            m_AutoBuffCnt--;
                            if (m_AutoBuffCnt <= 0)
                            {
                                m_AutoBuffCnt = m_AutoBuffInterval;
                                if (speedUpFrameNum < m_ClientFrameRateMultiple - 1)
                                {
                                    speed = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //正在缓冲中

                //剩下的可加速的帧数
                int speedUpFrameNum = newFrameNum - m_BuffSize;
                if (speedUpFrameNum > 0)
                {
                    //当缓冲的数量足够时，结束缓冲
                    m_IsInBuffing = false;
                }
            }

            m_IsInSpeedUp = speed > m_DefaultSpeed;
            return speed;
        }
    }
}
