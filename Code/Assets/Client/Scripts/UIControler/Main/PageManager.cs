using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public enum PageType
    { 
        FULL_SCREEN,
        COVER,
    }

    //页面管理器
    public class PageManager :MonoBehaviour
	{


        private static PageManager instance;

        public static PageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<PageManager>();
                }
                if (instance == null)
                {
                    instance = new GameObject("SystemConfig").AddComponent<PageManager>();
                }
                return instance;
            }

        }

        public List<Page> memoryPages;

        public Dictionary<string, Page> pageDictonary;

        protected List<string> pageHostory = new List<string>(); //操作栈

        public List<string> alwaysInMemoryAtlas;

        // 当前页面 
        public Page currentPage;


        void Awake()
        {
            InitPages();
        }

        private void InitPages()
        {
            pageDictonary = new Dictionary<string, Page>();
            foreach (Page p in memoryPages)
            {
                pageDictonary[p.name] = p;
                p.pageManager = this;
            }
        }
        
		public void DestoryMemoryPage()
		{
			foreach(Page page in memoryPages)
			{
				if(page == null)
				{
					Debug.LogError("memory page be destory in error mode");
				}
				else
				{
					page.OnMemoryPageDestory();
				}
			}

		}

		public void OpenPage(string pageName,string option)
		{
			Page pageToOp = GetPage(pageName);
			if (pageToOp == null)
			{
				Debug.LogError("Cannot find the page " + pageName);
				return;
			}
			
			if (currentPage != null)
			{
                if (currentPage.name == pageName)
                {
                    currentPage.Reopen(option);
                    return;
                }

				string pao = currentPage.name + "?" + currentPage.GetOptString();

                currentPage.OnUnactive ( );

                pageHostory.Add(pao);
				if (pageToOp.pageType == PageType.FULL_SCREEN)
				{
					for (int i = pageHostory.Count - 1; i >= 0; --i)
					{
						string pn = pageHostory[i];
						int indexQ = pn.IndexOf('?');
						if (indexQ != -1)
						{
							pn = pn.Substring(0, indexQ);
						}
						if (pageDictonary.ContainsKey(pn))
						{
							if (pageDictonary[pn].isOpen)
							{
                                pageDictonary[pn].OnForceClose();
								SimpleClosePage(pageDictonary[pn]);
							}
							else
							{
								break;
							}
						}
						else
						{
							break;
						}
					}
				}
			}
			currentPage = pageToOp;
			SimpleOpenPage(pageToOp,option);
            pageToOp.OnActive ( );
		
		}
		
		private Page GetPage(string pageName)
		{
			Page page = null;
			if (pageDictonary.ContainsKey(pageName))
			{
				page = pageDictonary[pageName];
			}
			else
			{
                Object obj = Resources.Load("Windows/" + pageName, typeof(GameObject));
				if (obj != null)
				{
					GameObject gObj = Instantiate(obj) as GameObject;
					gObj.transform.parent = transform;
					gObj.transform.localScale = Vector3.one;
					gObj.name = pageName;
					page = gObj.GetComponent<Page>();
					
					if (page != null)
					{
						pageDictonary[pageName] = page;
						page.pageManager = this;
                        if (page.alwaysInMemery)
                        {
                            memoryPages.Add(page);
                        }
                    }
                }
            }
            return page;
        }

        private void SimpleOpenPage(Page page, string option)
        {
            SceneManager.Instance.UI.GetComponent<UIController>().info.topTween.PlayReverse();
            SceneManager.Instance.UI.GetComponent<UIController>().info.bottomTween.PlayReverse();
			if(page.isOpen)
			{
				page.Reopen(option);
			}
			else
			{
				page.Open(option);
			}
            //activePage.Add(page);
        }

        public void CloseAllPage()
        {
            while (pageHostory.Count > 0)
            {
                currentPage.Close();
            }
            currentPage.Close();
        }

        private void SimpleClosePage(Page page)
        {
			page.Hide();
            foreach (UIAtlas ua in page.atlasOfPage)
            {
                bool cannotUnload = false;
                foreach (Page p in memoryPages)
                {
                    foreach (UIAtlas uia in p.atlasOfPage)
                    {
                        if (uia == ua)
                        {
                            cannotUnload = true;
                            break;
                        }
                    }
                    if (cannotUnload)
                    {
                        break;
                    }
                }
                if (!cannotUnload && alwaysInMemoryAtlas.IndexOf(ua.name) == -1)
                {
                    Resources.UnloadAsset(ua.spriteMaterial.mainTexture);
                }
            }
            foreach (UITexture ut in page.texturesOfPage)
            {
                bool cannotUnload = false;
                foreach (Page p in memoryPages)
                {
                    foreach (UITexture uim in p.texturesOfPage)
                    {
                        if (uim == ut)
                        {
                            cannotUnload = true;
                            break;
                        }
                    }
                    if (cannotUnload)
                    {
                        break;
                    }
                }
                if (!cannotUnload && alwaysInMemoryAtlas.IndexOf(ut.name) == -1)
                {
                    Resources.UnloadAsset(ut.mainTexture);
                }
            }

            if (!page.alwaysInMemery)
            {
                pageDictonary.Remove(page.name);
                Destroy(page.gameObject);
            }
		}
		
        public void OnClosePage(){
			if (pageHostory.Count == 0)
            {
                SceneManager.Instance.UI.GetComponent<UIController>().info.topTween.PlayForward();
                SceneManager.Instance.UI.GetComponent<UIController>().info.bottomTween.PlayForward();
                currentPage = null;
				return;
			}
			bool isCover = false;
			if (currentPage.pageType == PageType.COVER)
			{
				isCover = true;
			}
			string curPageName = currentPage.name;
			Page pageToClose = currentPage;
			
			string pao = pageHostory[pageHostory.Count - 1];
			pageHostory.RemoveAt(pageHostory.Count - 1);
			
			char[] sc = { '?' };
			string[] sep = pao.Split(sc);
			Page prePage = GetPage(sep[0]);
			currentPage = prePage;

			if(isCover)
			{
				SimpleClosePage(pageToClose);
				currentPage.OnCoverPageRemove();
				return ;            
			}

            SimpleClosePage(pageToClose);
			if (sep.Length == 2 && prePage != null)
			{
				SimpleOpenPage(prePage, sep[1]);
				
				if(prePage.pageType != PageType.FULL_SCREEN)
				{
					for (int i = pageHostory.Count - 1; i >= 0; --i)
					{
						string[] sep2 = pageHostory[i].Split(sc);
						if(sep2.Length != 2)
						{
							return;
						}
						Page popPage = GetPage(sep2[0]);
						SimpleOpenPage(popPage, sep2[1]);
						if (popPage.pageType == PageType.FULL_SCREEN)
						{
							break;
						}
					}
				}
				
			}
			else
			{
				Debug.LogError("page dictionary error");
			}
        }
       
		
		public void HideObject(GameObject obj)
        {
            obj.transform.localPosition = new Vector3();
        }

        public void AddPage(string pageName,Page page)
        {
            if (pageDictonary.ContainsKey(pageName))
            {
                Debug.LogError(pageName + "has already exist");
            }
            pageDictonary[pageName] = page;
        }
        public void RemovePage(string pageName)
        {
            if (!pageDictonary.ContainsKey(pageName))
            {
                Debug.LogError(pageName + "has not exist");
            }
            else
            {
                pageDictonary.Remove(pageName);
            }
        }


        private string[] ParseMailString(string mailContent)
        {
            return mailContent.Split(',');
        }

		public void HideUI()
		{
			transform.localPosition = new Vector3(10000f,10000f,10000f);
			gameObject.SetActive(false);
            //Resources.UnloadUnusedAssets();
		}
		public void ShowUI()
		{
			transform.localPosition = Vector3.zero;
			gameObject.SetActive(true);
			//masked by zhouwei
			//StartCoroutine(FreshTopAndSide());
		}
	

        public T GetPage<T>()where T:Page
        {
            T retPage = null;
            foreach (Page page in memoryPages)
            {
                retPage = page.gameObject.GetComponent<T>();
                if (retPage != null)
                {
                    return retPage;
                }
            }
            return retPage;

        }

	}
