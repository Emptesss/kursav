   M S B A M L   `   `   `  ����B  >CatGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:CatGameCatGame  5      7 clr-namespace:CatGame.ViewModelsCatGame.ViewModels  5      -clr-namespace:CatGame.ViewsCatGame.Views  5      1clr-namespace:CatGame.HelpersCatGame.Helpers  5      5clr-namespace:CatGame.BehaviorsCatGame.Behaviors  5          CatGame.App   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:CatGame     5      7
viewModels clr-namespace:CatGame.ViewModels     5      -viewsclr-namespace:CatGame.Views     5      1helpersclr-namespace:CatGame.Helpers     5      5	behaviorsclr-namespace:CatGame.Behaviors     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5	        �� 	Resources  5         MinecraftFont-   'CatGame.Helpers.PurchaseStatusConverter  PurchaseStatusConverter(   "CatGame.Helpers.BoolToBuyConverter  BoolToBuyConverter*   $CatGame.Helpers.InverseBoolConverter  InverseBoolConverter)   #CatGame.Helpers.IsSelectedConverter  IsSelectedConverter2   ,CatGame.Helpers.VisibilityToOpacityConverter   VisibilityToOpacityConverter-   'CatGame.Helpers.SelectedToTextConverter  SelectedToTextConverter)   #CatGame.Helpers.ImageScaleConverter  ImageScaleConverter �� 
TargetType �� Property �� Value �� BasedOn-   'CatGame.Behaviors.SoundBehaviorAttacher  
HeaderText/	   )CatGame.Helpers.BoolToVisibilityConverter 	 BoolToVisibility*
   $CatGame.Helpers.BoolToScaleConverter 
 BoolToScaleConverter*   $CatGame.Helpers.BoolToAngleConverter  BoolToAngleConverter0  *System.Windows.Media.Animation.ElasticEase  ElasticEase7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode   Oscillations   Springiness-  'System.Windows.Media.Animation.SineEase  SineEaseOut  SineEaseInOut2  ,System.Windows.Media.Animation.QuadraticEase  QuadraticEaseOut.  (System.Windows.Media.Animation.CubicEase  CubicEaseInOut  ElasticEaseOut  
SineEaseIn-  'System.Windows.Media.Animation.BackEase  BackEaseOut   	Amplitude/  )System.Windows.Media.Animation.BounceEase  BounceEaseSoft	   Bounces
   
Bounciness  BounceEaseOut/  )System.Windows.Media.Animation.CircleEase  CircleEaseOut  
BackEaseIn  CubicEaseIn  CubicEaseOut  PauseMenuStyle!   CatGame.Views.PauseMenuView  CatStyle  SkinImageStyle  MainButtonStyle  InvisibleButtonStyle �� 
TargetType �� Triggers P� Property P� Value  ImageButtonStyle
 � Tag   
FocusStyle �� DataType*   $CatGame.ViewModels.MainMenuViewModel    CatGame.Views.MainMenuView0   *CatGame.ViewModels.MainGameScreenViewModel&    CatGame.Views.MainGameScreenView&    CatGame.ViewModels.ShopViewModel   CatGame.Views.ShopView+   %CatGame.ViewModels.MiniGame1ViewModel!   CatGame.Views.MiniGame1View/   )CatGame.ViewModels.MiniGamesMenuViewModel%   CatGame.Views.MiniGamesMenuView+   %CatGame.ViewModels.PauseMenuViewModel*    $CatGame.ViewModels.GameOverViewModel !   CatGame.Views.GameOverView��%~  &	        &	 U     &	 h     &	 {     &	 �     &	 �     &	 �     &	 �     '�� �     7   '�� J    7��7   &	 �    7��&		 b    &	
 u    &	 �    &	 �    &	 �    &	     &	 .    &	 O    &	 r    &	 �    &	 �    &	 �    &	 +    &	 t    &	 �    &	 �    &	 �    &	     &	 �    &	 ;    &	 g    7��&	 �    7��&	 R
    &	  �    (�� �    5�      *, +)(�� -    5�      *, +)(�� _    5�      *, +)(�� �    5�      *, +)(�� �    5�      *, +)(�� �    5�      *, +)(�� '    5�      *,  +)*�5   
   A=pack://application:,,,/CatGame;component/Views/#Minecraft Rus)�6j     5   
   6*     5   
   6%     5   
   6'     5   
   6&     5   
   6/     5   
   6*     5   
   6&   �� 5   
   " ��6   .�5      ��  � ��8   6+   �� 5       � �� �	#FF2D2D2D6+   5      �� 5   
   " ��6   8   .�5      ��  �@ EnableSound$
 True��6L   �� 5       � ��8  6+   �� 5       � ��$ 24(�6)   5       �� 5"   
   " ��6#   8   .�5#      ��  � ��$ 72(�6)   �� 5$       � �� �	#FFF5649E6+   5%      	  5&   
   6,   
  5'   
   6'     5(   
   6'     5)   
   $ EaseOut=�5*      $ 1��5+      $ 3Q�5,        5.   
   $ EaseOut=�5/        51   
   $ 	EaseInOut=�52        53   
   $ EaseOut=�61     54   
   $ 	EaseInOut=�6+     55   
   $ EaseOut=�6-   $ 1��6B     56   
   $ EaseIn=�6&     57   
   $ EaseOut=�6'   $	 0.5Q�6<     58   
   $	 2��6,   $
 1Q�68   $ EaseOut=�6G     59   
   $ EaseOut=�5:      $	 2��5;      $	
 1.5Q�5<        5>   
   $ EaseOut=�5?        5A   
   $ EaseIn=�5B      $	 0.3Q�5C        5E   
   $ EaseIn=�5F        5H   
   $ EaseOut=�5I      �� 5L   
   " ��6'   .�5M      ��  � �� 5N      �� 5P      .R�5Q        5R      5T      5U      �� 5V   
   " ��6   .�5W      ��  � ��$  /Views/кoтправо.png��6'   �� 5X       � ��$	 200��6&   �� 5Y       � ��$	 200��6'   5Z      �� 5[   
   " ��6'   .�5\      ��  � ��$	 200��6&   �� 5]       � ��$	 200��6'   �� 5^       � ��$ UniformToFill=�6(   �� 5_       �@��BitmapScalingMode$ HighQuality=�6@   �� 5`       � ��$ Center=�64   �� 5a       � ��$ Center=�62   5b      �� 5c   
   " ��6(   8   .�5d      ��  � ��$	 300��6&   �� 5e       � ��$ 80��6'   �� 5f       � ��$ 10q�6'   �� 5g       � ��$ 24(�6)   �� 5h       � �� �	#FFF9BFCD6+   �� 5i       � �� �	#FFE9A5B56,   �� 5j       � ��$ 2q�60   5k      �� 5m   
   " ��6-   8   .�5n      ��  � ��
 ���� 6+   �� 5o       � ��
 ���� 6,   �� 5p       � ��$ 0q�60   �� 5q       � ��$ 0q�6(   �� 5r       � �� 5s      �� 5t      " ��6&   .R�5u      �� 
������ ._�5v      �� 5w      5x      5z       5{      P� 5|       �@J�IsMouseOver$
 True��61   . �5}      ��  �@�Cursor$
 Hand��6/   5~      5�      �� 5�   
   " ��6)   .�5�      ��  � �� 5�      �� 5�      " ��6&   .R�5�      � .+�5�      �� #��z 6$   $��Uniform=�5�      $	��180��5�      $��40��5�      5�      5�      5�      �� 5�       � ��
 ���� 6+   �� 5�       � ��$ 0q�60   �� 5�       � ��$ 0q�6(   5�      �� 5�   
   " ��6#   .�5�      ��  �@�FocusVisualStyle 61   Z� 5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "  6   .R�5�        5�      �� 5�   
   "   6   .R�5�      !  5�      5�      5�      