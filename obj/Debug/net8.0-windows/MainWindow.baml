   M S B A M L   `   `   `  ����R  NRealRegressionExercises, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:RealRegressionExercisesRealRegressionExercises  5   	   U QScottPlot.WPF, Version=5.0.47.0, Culture=neutral, PublicKeyToken=86698dc10387c39eD2clr-namespace:ScottPlot.WPF;assembly=ScottPlot.WPFScottPlot.WPF 5   	   (    "RealRegressionExercises.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:RealRegressionExercises     5   	   H	scottplot2clr-namespace:ScottPlot.WPF;assembly=ScottPlot.WPF     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��810��6)   .��5
      � $��	0,0,400,0q�6    � ColumnDefinitions 5   
   �� 5      $��4* �6   �� 5      $	��37* �6   �� 5      $��0* �6   �� 5      $��0* �6   .+�5   
     ScottPlot.WPF.WpfPlot  -   5          Name$ 	MyWpfPlot��5      $��
10,0,10,78q�6/   $��2��6C   � 5   
   $��10,0,0,0q�6   $
��Left=�6!   $	��810��6<    J� RenderTransformOrigin$ 0.178,0.458&�6H   $	��382��6l   $��Center=�6y   $��4��6�    5      �� 5      $	��83* �6#   �� 5      $	��79* �6#   5      �� 5   
   -   5        $��80,157,-330,212q�5      $��2��6*   $��2��6O   .��5      �� -   5         ��Name$ 
ButtonText��5      $
��Wrap=�6,   $��Input Data (.xlsx)��6@   5      �� 5   
   -   5        $��300,370,-394,18q�5      $��2��6+   $��2��6;   6O   �� 5   
   $��168,222,-222,189q�6   $��2��6.   $��2��6>   $�� ��6R   �� 5   
   $��320,382,-374,29q�6   $��2��6-   $��2��6=   �� 6S    � Language$ en-ca�6W   $�Y��6h   �� 6s   $�-Axis��6w   6�   �� 5   
   -   5        $��300,368,-394,18q�5      $��2��6+   $��2��6;   6O   �� 5   
   -   5        $ 	BottomBox��5      $��65,331,40,26q�6'   $��1��6=   $�� ��6M   �� 5    
   -   5        $ PopUpBox��5       $��128,245,-282,96q�6&   $��2��6?   $��2��6O   $�� ��6c   5!      5"      