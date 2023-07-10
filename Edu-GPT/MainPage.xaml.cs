using CommunityToolkit.Maui.Markup;

namespace Edu_GPT;



public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		Content = new VerticalStackLayout()
		{
			Children = new Label().Font(size: 50).Center()
        };
	}

	
}


