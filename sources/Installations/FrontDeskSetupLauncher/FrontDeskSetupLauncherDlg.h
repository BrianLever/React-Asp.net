	
// FrontDeskSetupLauncherDlg.h : header file
//

#pragma once
#include "afxwin.h"
#include "stdafx.h"



// CFrontDeskSetupLauncherDlg dialog
class CFrontDeskSetupLauncherDlg : public CDialog
{
// Construction
public:
	CFrontDeskSetupLauncherDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_FRONTDESKSETUPLAUNCHER_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;
	
	CBitmap bitmapBkgnd;

	// Button texts and setup commands, read from setup.ini file. 
	CString buttonText1;
	CString buttonText2;
	CString programName1;
	CString programName2;
	CString programParameters1;
	CString programParameters2;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	
	BOOL CFrontDeskSetupLauncherDlg::IsFramework35Installed();
	//void CFrontDeskSetupLauncherDlg::RunFile(LPCWSTR filename, LPWSTR args);
	void CFrontDeskSetupLauncherDlg::RunFile(LPCWSTR filename, LPCWSTR args);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();

	bool ReadConfiguration();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	CStatic m_PictureFrame;
	afx_msg void OnBnClickedInstall();
	afx_msg void OnBnClickedServer();
};
