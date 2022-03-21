
// FrontDeskSetupLauncherDlg.cpp : implementation file
//

#include "stdafx.h"
#include "stdio.h"
#include "FrontDeskSetupLauncher.h"
#include "FrontDeskSetupLauncherDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CFrontDeskSetupLauncherDlg dialog


CFrontDeskSetupLauncherDlg::CFrontDeskSetupLauncherDlg(CWnd* pParent /*=NULL*/)
: CDialog(CFrontDeskSetupLauncherDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDI_ICON1);
	bitmapBkgnd.Attach(LoadBitmap (AfxGetInstanceHandle(), 
		MAKEINTRESOURCE(IDB_BITMAP3)));


}

void CFrontDeskSetupLauncherDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CFrontDeskSetupLauncherDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_WM_ERASEBKGND()
	ON_BN_CLICKED(IDINSTALL, &CFrontDeskSetupLauncherDlg::OnBnClickedInstall)
	ON_BN_CLICKED(IDSERVER, &CFrontDeskSetupLauncherDlg::OnBnClickedServer)
END_MESSAGE_MAP()


// CFrontDeskSetupLauncherDlg message handlers

BOOL CFrontDeskSetupLauncherDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	//check if .net framework 3.5 is installed
	if(!IsFramework35Installed())
	{
		//run framework install
		RunFile(_T("DotNetFX35SP1\\dotNetFx35setup.exe"), NULL);
	}

	if(!ReadConfiguration()){
		// just disable buttons. Let user see "default text" on buttons.
		GetDlgItem(IDINSTALL)->EnableWindow(FALSE);
		GetDlgItem(IDSERVER)->EnableWindow(FALSE);
	}
	else{
		// set text on buttons
		GetDlgItem(IDINSTALL)->SetWindowTextW(this->buttonText1);
		GetDlgItem(IDSERVER)->SetWindowTextW(this->buttonText2);
	}

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CFrontDeskSetupLauncherDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CFrontDeskSetupLauncherDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CFrontDeskSetupLauncherDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	OnOK();
}

BOOL CFrontDeskSetupLauncherDlg::OnEraseBkgnd(CDC* pDC) 
{
	CRect rect;
	GetClientRect(&rect);
	CDC dc;
	dc.CreateCompatibleDC(pDC);
	CBitmap* pOldBitmap = dc.SelectObject(&bitmapBkgnd);
	int iBitmapWidth, iBitmapHeight ;
	int ixOrg, iyOrg;

	BITMAP bm;
	bitmapBkgnd.GetObject(sizeof(bm),&bm);
	iBitmapWidth = bm.bmWidth;
	iBitmapHeight = bm.bmHeight;

	// If our bitmap is smaller than the background and tiling is
	// supported, tile it. Otherwise watch the efficiency - don't
	// spend time setting up loops you won't need.

	if (iBitmapWidth  >= rect.Width() &&
		iBitmapHeight >= rect.Height() )
	{
		pDC->BitBlt (rect.left, 
			rect.top, 
			rect.Width(),
			rect.Height(),
			&dc,
			0, 0, SRCCOPY);
	}
	else
	{
		for (iyOrg = 0; iyOrg < rect.Height(); iyOrg += iBitmapHeight)
		{
			for (ixOrg = 0; ixOrg < rect.Width(); ixOrg += iBitmapWidth)
			{
				pDC->BitBlt (ixOrg, 
					iyOrg, 
					rect.Width(),
					rect.Height(),
					&dc,
					0, 0, SRCCOPY);
			}
		}
	}

	dc.SelectObject(pOldBitmap);
	return TRUE;
}


BOOL CFrontDeskSetupLauncherDlg::IsFramework35Installed()
{
	CRegKey* regKey = new CRegKey();
	/*TCHAR szBuffer[256];
	ULONG cchBuffer = 256;*/
	REGSAM PrimarySecAccMask = KEY_QUERY_VALUE  ;
	LONG result = regKey->Open(HKEY_LOCAL_MACHINE, _T("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.5"), PrimarySecAccMask);

	regKey->Close();
	delete regKey;
	if(result == ERROR_SUCCESS ){
		return TRUE;
	}
	else return FALSE;
}



void CFrontDeskSetupLauncherDlg::OnBnClickedInstall()
{
	//RunFile(_T("SQLServerInstaller.exe"), NULL);

	RunFile(this->programName1, NULL);
}

void CFrontDeskSetupLauncherDlg::OnBnClickedServer()
{
/*
	//	            RunFile(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.System), "msiexec.exe"), "/i FrontDeskServerSetup.msi");

	CAtlString windowFolder;
	windowFolder.GetEnvironmentVariableW(_T("WINDIR"));
	CString path = windowFolder + "\\system32\\msiexec.exe";
	//AfxMessageBox(path);

	_TCHAR buf[MAX_PATH];

	GetCurrentDirectory( _MAX_PATH, buf); 

	CString pathToMsi;
	pathToMsi.FormatMessage(_T("/i \"%1\\FrontDeskServerSetup.msi\""), buf);

	//AfxMessageBox(pathToMsi);
	RunFile(path,pathToMsi.GetBuffer());

	pathToMsi.ReleaseBuffer();
*/

	RunFile(this->programName2, this->programParameters2);
}

//void CFrontDeskSetupLauncherDlg::RunFile(LPCWSTR filename, LPWSTR args)
void CFrontDeskSetupLauncherDlg::RunFile(LPCWSTR filename, LPCWSTR args)
{

	/*STARTUPINFO si;
	PROCESS_INFORMATION pi;

	ZeroMemory( &si, sizeof(si) );
	si.cb = sizeof(si);
	ZeroMemory( &pi, sizeof(pi) );*/


	SHELLEXECUTEINFO ExecuteInfo;

	memset(&ExecuteInfo, 0, sizeof(ExecuteInfo));
	ExecuteInfo.cbSize       = sizeof(ExecuteInfo);
	ExecuteInfo.fMask        = 0;                
	ExecuteInfo.hwnd         = 0;                
	ExecuteInfo.lpVerb       = NULL;
	ExecuteInfo.lpFile       = filename;
	ExecuteInfo.lpParameters = args;
	ExecuteInfo.lpDirectory  = 0; 
	ExecuteInfo.nShow        = SW_SHOW;
	ExecuteInfo.hInstApp     = 0;

	ShellExecuteEx(&ExecuteInfo);

	//try
	//{
	//	// Start the child process. 
	//	if( !CreateProcess(filename ,
	//		args,        // Command line
	//		NULL,           // Process handle not inheritable
	//		NULL,           // Thread handle not inheritable
	//		FALSE,          // Set handle inheritance to FALSE
	//		0,              // No creation flags
	//		NULL,           // Use parent's environment block
	//		NULL,           // Use parent's starting directory 
	//		&si,            // Pointer to STARTUPINFO structure
	//		&pi )           // Pointer to PROCESS_INFORMATION structure
	//		) 
	//	{
	//		printf( "CreateProcess failed (%d).\n", GetLastError() );
	//		return;
	//	}

	//	// Wait until child process exits.
	//	WaitForSingleObject( pi.hProcess, INFINITE );

	//	// Close process and thread handles. 
	//	CloseHandle( pi.hProcess );
	//	CloseHandle( pi.hThread );
	//	//delete buf;



	//}
	//catch (CException* ex)
	//{
	//	TCHAR   szCause[255];
	//	CString strFormatted;

	//	ex->GetErrorMessage(szCause, 255);


	//	strFormatted = _T("Operation failed. Error message: ");
	//	strFormatted += szCause;


	//	AfxMessageBox(strFormatted);
	//}

}

bool CFrontDeskSetupLauncherDlg::ReadConfiguration(){
	CStdioFile iniFile;
	CFileException ex;

	if(!iniFile.Open(_T("setup.ini"), CFile::modeRead, &ex)){
		 TCHAR szError[1024];
		 TCHAR szMessage[2048];
		 ex.GetErrorMessage(szError, 1024);
		 //_tprintf_s(_T("Couldn't open source file: %1024s"), szError);
		 _stprintf_s(szMessage, 2048, _T("Couldn't open setup configuration file: %s"), szError);
		 
		 AfxMessageBox(szMessage);

		 iniFile.Close();	// causes assert in Debug - place after messagebox

		 return false;
	}

	CString line;
	int processedLines = 0;	// ugly: search only for 2 lines.
	while(iniFile.ReadString(line) && (processedLines < 2)){
		if(line.GetAt(0) == _T('#')){
			// skip comments
			continue;
		}

		int curPos = 0;
		CString buttonName = line.Tokenize(_T("|"), curPos);
		if(curPos == -1) {continue;}	// bad string
		CString programName = line.Tokenize(_T("|"), curPos);
		if(curPos == -1) {continue;}	// bad string
		CString programParameters = line.Tokenize(_T("|"), curPos);
		//if(curPos == -1) {continue;}	// parameters are optional
		processedLines++;

		switch(processedLines){
			case 1:
				this->buttonText1 = buttonName;
				this->programName1 = programName;
				this->programParameters1 = programParameters;
				break;
			case 2:
				this->buttonText2 = buttonName;
				this->programName2 = programName;
				this->programParameters2 = programParameters;
				break;
		}

		//int delimPos = line.Find(_T('='));
		//if(delimPos != -1){
		//	CString key = line.Mid(0, delimPos).Trim();
		//	CString value = line.Mid(delimPos + 1).Trim();
		//	if(key == _T("button1")){
		//		this->buttonText1 = value;
		//	}
		//	else if(key == _T("button2")){
		//		this->buttonText2 = value;
		//	}
		//	else if(key == _T("command1")){
		//		this->commandText1 = value;
		//	}
		//	else if(key == _T("command2")){
		//		this->commandText2 = value;
		//	}
		//}
	}

	iniFile.Close();

	return true;
}