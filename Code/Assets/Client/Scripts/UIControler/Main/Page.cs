/// 每个页面都含有自己单独的optionString;
/// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class Page : MonoBehaviour,IHidable
	{
        public PageType pageType = PageType.FULL_SCREEN;

        public UIAtlas[] atlasOfPage;
        public UITexture[] texturesOfPage;

        /// <summary>
        /// 是否永久驻留在内存
        /// </summary>
        public bool alwaysInMemery = true;

        /// <summary>
        /// 是否需要默认背景
        /// </summary>
        public bool needDefaultBack = true;

        /// <summary>
        /// 页面实现的初始位置
        /// </summary>
        public Vector3 initPosition = Vector3.zero;
        public PageManager pageManager;
		//操作字符串
		protected string optionString;
		public string OptionString
		{
			get{ return optionString;}
			set{ optionString = value;}
		}
		// 存放解析字符串
		public Dictionary<string,string> options = new Dictionary<string, string> ();

		// 页面是否被打开
		public bool isOpen=false;
		// 重新设置操作字符串
		public string GetOptString()
		{
            SaveOptString();
            optionString = "";
			foreach( KeyValuePair<string,string> opt in options )
			{
				optionString += opt.Key + "=" + opt.Value +"&" ;
			}
            return optionString;
		}

        //将显示逻辑 转化成字符串键值对
        protected virtual void SaveOptString()
        { 
            
        }

        public virtual void OnActive()
        {
           // Debug.Log ( ":active="+name );
        }

        public virtual void OnUnactive ( )
        {
         //   Debug.Log ( ":nonactive="+name );
        }

		// 解析操作字符串
		protected void ParseOptString()	//逻辑
		{
            options.Clear();
			string[] strArr = optionString.Split(new char[1]{'&'},System.StringSplitOptions.RemoveEmptyEntries);
			foreach(string s in strArr)
			{
				if(s.Contains("="))
				{
					string[] strV = s.Split(new char[1]{'='});
                   
                    if(options.ContainsKey(strV[0]))
                    {
                        options[strV[0]] = strV[1];
                    }
                    else
                    {
                        options.Add(strV[0], strV[1]);
                    }
                  
				}
			}
		}

        //打开一个页面
        public void Open(string options)
        {
            this.optionString = options;
            isOpen = true;
            ParseOptString();
            Show();
            DoOpen();
        }

        //有子类中覆盖的打开方法，进行字符串键值对转化成显示逻辑
        protected virtual void DoOpen()
        {
        }

        public virtual void OnForceClose()
        { 
            
        }

        //关闭页面，同时通知PageManager关闭当前页面
        public virtual void Close()
        {
            isOpen = false;
            Hide();
            DoClose();
            PageManager.Instance.OnClosePage();
        }

        public virtual void Reopen(string options)
        {
            this.optionString = options;
            ParseOptString();
            Debug.Log("reopen");
            DoReopen();
        }

        public virtual void DoReopen()
        { 
            
        }

        //有子类中覆盖的关闭方法，可以进行一些特定操作
        protected virtual void DoClose()
        {
            
        }

        //设置页面某一参数的值
        public virtual void SetOptionValue(string key,string value)
        {
            options[key] = value;
        }

        //隐藏当前页面
        public virtual void Hide()
        {
            //transform.localPosition = new Vector3(-10000f, 10000f, 10000f);
            gameObject.SetActive(false);
            isOpen = false;
        }

        //显示当前页面
        public virtual void Show()
        {
            transform.localPosition = initPosition;
            gameObject.SetActive(true);
            isOpen = true;
        }
        
        public virtual void OnCoverPageRemove()
        {
        	Debug.Log("Cover page remove");
        
        }
		public virtual void OnMemoryPageDestory()
		{
			Debug.Log("Destory from memory!");
		}
	}