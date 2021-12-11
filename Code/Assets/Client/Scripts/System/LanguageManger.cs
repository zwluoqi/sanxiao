using System;
using System.Collections.Generic;
using GCGame.Table;


public enum LanguageType
{
	LANGUAGE_CHINESE = 0,
	LANGUAGE_ENGLISH = 1,
}

public class LanguageManger
{	
	private LanguageType mLangType;
	public void SetLangType(LanguageType type)
	{
		mLangType = type;
	}
	public LanguageType GetLangType() {return mLangType;}
	
	private LanguageManger()
	{
//		mLangType = LanguageType.LANGUAGE_CHINESE;
//		Localization.language = "chinese";
	}

	public void ExchangeLanguage(LanguageType type){
		mLangType = type;
		if (type == LanguageType.LANGUAGE_CHINESE) {
			Localization.language = "chinese";
		} else {
			Localization.language = "english";
		}
	}

	private static LanguageManger mInstance = null;
	public static LanguageManger GetMe()
	{
		if(mInstance == null)
		{
			mInstance = new LanguageManger();
		}
		return mInstance;
	}
	public string GetWords(string id)
	{
        if (id != null && id != "")
        {
            return Localization.Get(id);
        }
        else
        {
            return "";
        }
	}
}

