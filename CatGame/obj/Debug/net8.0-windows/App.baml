   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CatGameCatGame  5      7 clr-namespace:CatGame.ViewModelsCatGame.ViewModels  5      -clr-namespace:CatGame.ViewsCatGame.Views  5          CatGame.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:CatGame     5      7
viewModels clr-namespace:CatGame.ViewModels     5      -viewsclr-namespace:CatGame.Views     5        �� 	Resources  5         MainButtonStyle �� 
TargetType �� Property �� Value  InvisibleButtonStyle �� 
TargetType �� Triggers P� Property P� Value �� DataType*   $CatGame.ViewModels.MainMenuViewModel    CatGame.Views.MainMenuView0   *CatGame.ViewModels.MainGameScreenViewModel&    CatGame.Views.MainGameScreenView&    CatGame.ViewModels.ShopViewModel   CatGame.Views.ShopView+   %CatGame.ViewModels.MiniGame1ViewModel!   CatGame.Views.MiniGame1View/	   )CatGame.ViewModels.MiniGamesMenuViewModel%
   CatGame.Views.MiniGamesMenuView��%l  &	        &	 0    (�� �    5%      *, +)(��     5(      *, +)(�� E    5+      *, +)(�� w    5.      *, +)(�� �    51      *,	 +)�� 5   
   " ��6(   .�5	      ��  � ��$	 300��6&   �� 5
       � ��$ 80��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 24(�6)   �� 5       � �� �	#FFF9BFCD6+   �� 5       � �� �	#FFE9A5B56,   �� 5       � ��$ 2q�60   5      �� 5   
   " ��6-   .�5      ��  � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � ��$ 0q�60   �� 5       � ��$ 0q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� 
������ ._�5      �� 5      5      5       5      P� 5        �@J�IsMouseOver$
 True��61   . �5!      ��  �@�Cursor$
 Hand��6/   5"      5$      �� 5%   
   "  6   .R�5&        5'      �� 5(   
   "  6   .R�5)        5*      �� 5+   
   "  6   .R�5,        5-      �� 5.   
   "  6   .R�5/        50      �� 51   
   " 	 6   .R�52      
  53      54      55      