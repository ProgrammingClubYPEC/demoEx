   M S B A M L   `   `   `  ����E  AProductsIS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ProductsIS
ProductsIS  5   	       ProductsIS.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ProductsIS     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 	Resources 5	        ProductsIS.ImageConvertet   imageConverter"  ProductsIS.ListToStrMaterial  materialConverter*  $ProductsIS.SummCostMaterialConverter  summConverter&   ProductsIS.TypeMaterialConverter  typeMaterialConverter��%t   &	        &	      &	 &     &	 9       5
   
   68     5   
   6>     5   
   6B     5   
   6F   5      .��5      � .+�5   
   �� -   5         ��Name$ listBox��5       �� SelectionMode$ Multiple=�6#   $
��Left=�6<   $	��293��6W   $��	10,53,0,0q�6d   $	��Top=�6w   $	��774��6�   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $��20q�6    $	��100��6p   ��6,   �� *Image+ �� 	Converter# [  6~   �� 5      $	��300��6%   $ Vertical=�61   .+�5      �� $��14(�6(   $
��Bold"�66   $��0,20,0,0q�6H   �� 6[   �6_   �� *ProductType+# [ 6�    - �� 6�   �6�   �� *Title+6�   6�   �� 5      $��0,0,0,5q�6G   ��6(   �� *ArticleNumber+6Z   �� 5      $
��Wrap=�6(   Материалы:�� 6G   �6K   �� *ProductMaterial+# [ 6�   6�   5      �� 5      $��0,20,0,0q�6$   $	��250��66    �� TextAlignment$ Right=�6B   Стоимость: �� 6d   �6h   �� *ProductMaterial+# [ 6�   6�   5      5      5      �� 5   
   -   5         ��Name$ navigationStackPanel��5      $y�Hidden=�63   $ 
Horizontal=�6G   $��305,364,268.6,27q�6`   .+�5       �� -   5         ��Name$ backBtn��5       .��6=   
Назад6D   � 5!      -   5        	 �Name$	 	wrapPanel��5!      $
��auto��6+   6;   �� 5"      -   5        $ nextBtn��5"      .��6<   Вперед6D   5#      �� 5$   
   -   5        $��filterComboBox��5$      $
��Left=�6)   $��	10,26,0,0q�6D   $	��Top=�6W   $	��120��6o   ��5%      �� 5&      .R�5'      �� $ 
Horizontal=�6!   .+�5(      �� -   5        5(   _   �� 5)      ��6$   �� *Title+6=   5*      5+      5-      �� 5.   
   -   5        
 ��Name$
 searchTextBox��5.      $
��Left=�6*   $��23��6E   $��
145,25,0,0q�6Q    �� TextWrapping$
 Wrap=�6e   $�� ��6y   $	��Top=�6�   $	��240��6�   6�   �� 5/   
   -	   5         ��Name$ sortComboBox��5/      $
��Left=�6)   $��
405,26,0,0q�6D   $	��Top=�6X   $	��120��6p   ��50      �� 51      .R�52      �� ��6    �� *Name+68   53      55      56      57      