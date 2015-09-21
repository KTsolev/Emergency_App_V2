package md5009fb10457b89106920badd8694a9c67;


public class MyOnTouchEventHandler
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.view.View.OnTouchListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onTouch:(Landroid/view/View;Landroid/view/MotionEvent;)Z:GetOnTouch_Landroid_view_View_Landroid_view_MotionEvent_Handler:Android.Views.View/IOnTouchListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("EmergencyApp_v2.MyOnTouchEventHandler, EmergencyApp_v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyOnTouchEventHandler.class, __md_methods);
	}


	public MyOnTouchEventHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyOnTouchEventHandler.class)
			mono.android.TypeManager.Activate ("EmergencyApp_v2.MyOnTouchEventHandler, EmergencyApp_v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public boolean onTouch (android.view.View p0, android.view.MotionEvent p1)
	{
		return n_onTouch (p0, p1);
	}

	private native boolean n_onTouch (android.view.View p0, android.view.MotionEvent p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
