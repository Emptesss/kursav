   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CatGameCatGame  5      7 clr-namespace:CatGame.ViewModelsCatGame.ViewModels  5      -clr-namespace:CatGame.ViewsCatGame.Views  5      1clr-namespace:CatGame.HelpersCatGame.Helpers  5          CatGame.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:CatGame     5      7
viewModels clr-namespace:CatGame.ViewModels     5      -viewsclr-namespace:CatGame.Views     5      1helpersclr-namespace:CatGame.Helpers     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5        �� 	Resources  5      /   )CatGame.Helpers.BoolToVisibilityConverter   BoolToVisibility  PauseMenuStyle �� 
TargetType �� Property �� Value!   CatGame.Views.PauseMenuView  MainButtonStyle  InvisibleButtonStyle �� 
TargetType �� Triggers P� Property P� Value  ImageButtonStyle
 � Tag	 �� DataType*   $CatGame.ViewModels.MainMenuViewModel    CatGame.Views.MainMenuView0   *CatGame.ViewModels.MainGameScreenViewModel&    CatGame.Views.MainGameScreenView&    CatGame.ViewModels.ShopViewModel   CatGame.Views.ShopView+	   %CatGame.ViewModels.MiniGame1ViewModel!
   CatGame.Views.MiniGame1View/   )CatGame.ViewModels.MiniGamesMenuViewModel%   CatGame.Views.MiniGamesMenuView��%e  &	        &	      &	 �     &	 �    &	 l    (�� �    5L      *, +)(�� �    5P      *, +)(��      5T      *, +)(�� R    5X      *,	 +)(�� �    5\      *, +)  5   
   6,   �� 5   
   " ��6'   .�5      ��  � �� 5      �� 5      .R�5        5      5      5      �� 5   
   " ��6(   .�5      ��  � ��$	 300��6&   �� 5       � ��$ 80��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 24(�6)   �� 5       � �� �	#FFF9BFCD6+   �� 5        � �� �	#FFE9A5B56,   �� 5!       � ��$ 2q�60   5"      �� 5$   
   " ��6-   .�5%      ��  � ��
 ���� 6+   �� 5&       � ��
 ���� 6,   �� 5'       � ��$ 0q�60   �� 5(       � ��$ 0q�6(   �� 5)       � �� 5*      �� 5+      " ��6&   .R�5,      �� 
������ ._�5-      �� 5.      5/      51       52      P� 53       �@J�IsMouseOver$
 True��61   . �54      ��  �@�Cursor$
 Hand��6/   55      57      �� 59   
   " ��6)   .�5:      ��  � �� 5;      �� 5<      " ��6&   .R�5=      � .+�5>      �� #��z 6$   $��Uniform=�5?      $	��180��5@      $��40��5A      5B      5C      5E      �� 5F       � ��
 ���� 6+   �� 5G       � ��$ 0q�60   �� 5H       � ��$ 0q�6(   5I      �� 5L   
   "	  6   .R�5M        5N      �� 5P   
   "	  6   .R�5Q        5R      �� 5T   
   "	  6   .R�5U        5V      �� 5X   
   "	 	 6   .R�5Y      
  5Z      �� 5\   
   "	  6   .R�5]        5^      5_      5`      