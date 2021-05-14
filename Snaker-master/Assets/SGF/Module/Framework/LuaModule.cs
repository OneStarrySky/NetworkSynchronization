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

using SGF;

namespace SGF.Module.Framework
{
    public class LuaModule : BusinessModule
    {
        private object m_args;

        /// <summary>
        /// 构造函数传入Name
        /// 是因为Lua模块无法通过反射来获取Name
        /// </summary>
        /// <param name="name"></param>
        internal LuaModule(string name) : base(name) { }


        /// <summary>
        /// 这里应该去加载Lua脚本
        /// 并且将EventManager映射到Lua脚本中
        /// </summary>
        /// <param name="args"></param>
        public override void Create(object args = null)
        {
            base.Create(args);
            this.Log("Create() Lua = " + Name);
            m_args = args;

            EventTable mgrEvent = GetEventTable();
            //TODO 需要映射到Lua脚本中
        }



        /// <summary>
        /// 调用它以卸载Lua脚本
        /// </summary>
        public override void Release()
        {
            base.Release();
            this.Log("Release() Lua = " + Name);
        }
    }
}
