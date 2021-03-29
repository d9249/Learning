import sys
import os
from PyQt5.QtWidgets import *
from PyQt5 import uic, QtCore
from PyQt5.QtWidgets import QApplication, QWidget, QFileDialog
from PyQt5.QtCore import QPoint, QRect
import urllib.request
from PyQt5.QtGui  import *
import numpy as np
import cv2

sys.path.append('.')
from MyWork.Util.imageutil import *
from MyWork.Util.ImageManager import ImageManager

form_class = uic.loadUiType("MyWork/ImageViewer/ImageViewer.ui")[0]



class WindowClass(QMainWindow, form_class):

    default_path = "Z:\\OpenDataSet\\(20190831)height_map2\\test"      # 한글 들어가면 잘 안됨...

    def __init__(self):
        super().__init__()
        self.setupUi(self)
        self.ratio = 1      # zoom : 1 - 100%, 2 - 200%
        self.btnFolders     = [self.pushBtnFolder1, self.pushBtnFolder2, self.pushBtnFolder3, self.pushBtnFolder4]
        self.widgetLabels   = [self.label1, self.label2, self.label3, self.label4]
        self.widgetImages   = [self.image1, self.image2, self.image3, self.image4]
        self.widgetChecks   = [self.checkBox1, self.checkBox2, self.checkBox3, self.checkBox4]
        self.listFolders    = ["", "", "", ""]
        self.labelDefaultPath.setText(self.default_path)
        self.horizontalSliderZoom.valueChanged.connect(self.changeZoom)
        self.leImageName.editingFinished.connect(self.changeImageName)

        for i, btn in enumerate(self.btnFolders):
            print ("Clicked", i, btn)
            self.btnFolders[i].clicked.connect(self.clickSelFolder)
            self.widgetLabels[i].setText(self.listFolders[i])
            self.widgetChecks[i].clicked.connect(self.clickCheck)

        self.changeZoom()
        '''
        self.image2.setGeometry(QtCore.QRect(x+400, y, 400, 400))     # QRect(x, y, width, height))
        '''

    def changeZoom(self):
        x = 10
        y = 150
        w = 400 
        h = 400
        hm = 3
        vm = 3
        if self.ratio == 1: 
            self.ratio = 2
        else:
            self.ratio = 1
        self.image1.setGeometry(QtCore.QRect(x, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))
        self.image2.setGeometry(QtCore.QRect(x+(w * self.ratio) + hm, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))
        self.blendimage1.setGeometry(QtCore.QRect(x+(w * self.ratio * 2) + 2*hm, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))

        y = y + (h * self.ratio) + vm
        self.image3.setGeometry(QtCore.QRect(x, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))
        self.image4.setGeometry(QtCore.QRect(x+(w * self.ratio) + hm, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))
        self.blendimage2.setGeometry(QtCore.QRect(x+(w * self.ratio * 2) + 2*hm, y, w * self.ratio, h * self.ratio))     # QRect(x, y, width, height))

    def clickCheck(self):
        idx = self.widgetChecks.index(self.sender())
        print(idx, self.widgetChecks[idx])
        if self.widgetChecks[0].isChecked() == True and self.widgetChecks[1].isChecked() == True:
            self.blendImageByOpenCV(0, 1)

    def blendImageByOpenCV(self, src1, src2):
        path1 = self.listFolders[src1]
        path2 = self.listFolders[src2]
        if path1 == "" or path2 == "": 
            return
        filepath1 = path1 + "/" + self.leImageName.text() + ".png"        
        filepath2 = path1 + "/" + self.leImageName.text() + ".png"        
        cvimg1 = cv2.imread(filepath1)
        cvimg2 = cv2.imread(filepath2)
        if cvimg1 is None or cvimg2 is None: 
            return
        # [blend_images]
        alpha = self.sliderAlpha.value() / 10.0
        beta = (1.0 - alpha)
        cvblendimg = cv2.addWeighted(cvimg1, alpha, cvimg2, beta, 0.0)
        height, width, byteValue = cvblendimg.shape
        if len(cvblendimg.shape) == 3:
            if(cvblendimg.shape[2])==4:
                qformat=QImage.Format_RGBA8888
            else:
                qformat=QImage.Format_RGB888
        cv2.imshow('blending', cvblendimg)
        self.mQimageA = QImage(cvblendimg.data, cvblendimg.shape[1], cvblendimg.shape[0], cvblendimg.strides[0], qformat)
        self.mQimageA = self.mQimageA.rgbSwapped()
        pixmap = QPixmap.fromImage(self.mQimageA)
        pixmap = pixmap.scaled(800,800, QtCore.Qt.KeepAspectRatio)
        self.blendimage1.setPixmap(pixmap) #QPixmap.fromImage(self.mQimageA))
        cv2.waitKey(0)


    def refreshImageByOpenCV(self):
        print("RefreshImageByOpenCV")
        for idx in range(4):
            path = self.listFolders[idx]
            if path == "": 
                continue
            filepath = path + "/" + self.leImageName.text() + ".png"
            print(filepath)
#            filepath = "Z:\OpenDataSet\ISTD\90-1.png"

            if os.path.exists(filepath):
                cvImage = cv2.imread(filepath)
                print("cvImage:",cvImage)
                height, width, byteValue = cvImage.shape
                if len(cvImage.shape) == 3:
                    if(cvImage.shape[2])==4:
                        qformat=QImage.Format_RGBA8888
                    else:
                        qformat=QImage.Format_RGB888
                self.mQimageA = QImage(cvImage.data, cvImage.shape[1], cvImage.shape[0], cvImage.strides[0], qformat)
                self.mQimageA = self.mQimageA.rgbSwapped()
                pixmap = QPixmap.fromImage(self.mQimageA)
                pixmap = pixmap.scaled(800,800, QtCore.Qt.KeepAspectRatio)
                self.widgetImages[idx].setPixmap(pixmap) #QPixmap.fromImage(self.mQimageA))
#            self.widgetImages[idx].setPixmap(QPixmap.fromImage(self.mQimageA))
            else:
                self.widgetImages[idx].setText("None")
        
    def changeImageName(self):
        print(self.leImageName.text())
        self.refreshImageByOpenCV()

    def clickSelFolder(self):
#        print("Button P{}".format(self.btnFolders.index(self.sender())))
        idx = self.btnFolders.index(self.sender())
        dialog = QFileDialog()
        folder = dialog.getExistingDirectory(self, 'Select an awesome directory', self.default_path)
        print("Select Folder({}): {}".format(idx, folder))
        if not folder == self.listFolders[idx]:
            '''
            refresh
            '''
            last_folder = folder.split('/')[-1]
            print("Changed Folder", folder)
            self.default_path, subfolder = os.path.split(folder)     #default_Path + '\\' + subfolder + '\\'
            self.labelDefaultPath.setText(self.default_path)
            self.listFolders[idx]  = folder
            self.widgetLabels[idx].setText(folder)
        else:
            print("No changed - Folder")















if __name__ == "__main__" :
    #QApplication : 프로그램을 실행시켜주는 클래스
    app = QApplication(sys.argv) 

    #WindowClass의 인스턴스 생성
    myWindow = WindowClass() 

    #프로그램 화면을 보여주는 코드
    myWindow.show()

    #프로그램을 이벤트루프로 진입시키는(프로그램을 작동시키는) 코드

    app.exec_()