import styled, { css }  from 'styled-components';
import { makeStyles  } from '@material-ui/core/styles';
import backgroundImage from '../../assets/circle-plus.svg';
import { MenuItem, Select } from '@material-ui/core';


export const pageTitleTextStyle = css `
  font-size: 25px;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  `;

export const reportHeaderTextStyle = css `
  font-size:16px;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  `;

export const baseTextStyle = css `
  font-size: 14px;
  line-height: 1.4;
`;



export const reportTextStyle = css `
  ${baseTextStyle}
  font-weight: 400;
  line-height: 1.4;
  `;



export const ContentContainer = styled.div`
  display: flex;
  padding: 45px;
  background-color: #fff;
  flex-direction: column;
  // border-radius: 102px;
  border: 1px solid transparent;
`;


export const TitleText = styled.h1`
  ${pageTitleTextStyle};
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TitleTextModal = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 19.2px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: #2e2e42;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TopSection = styled.div`
  background-color: #ededf2;
  font-size: 14px;
  border-radius: 5px;
  padding: 16px; 16px; 16px; 16px;;
  background-color: #ededf2;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-bottom: 10px;
`;

export const MiddleSection = styled.div`
  font-size: 14px;;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  border-radius: 5px;
  padding: 16px; 16px; 16px; 16px;;
  background-color: #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-bottom: 10px;
`;


export const TopSectionLabel = styled.h1`
  font-size: 14px;;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: none;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TopSectionText = styled.div`
  font-size: 14px;;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-transform: none;
  color: #2e2e42;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const AnswerSection = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  border: 1px solid #f5f6f8;
  border-radius: 0 0 5px 5px;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const AnswerSectionMiddle = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  border: 1px solid #f5f6f8;
  background-color: #f5f6f8;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const TopSectionAdditionalText = styled.div`
  font-size: 16px;;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-transform: none;
  color: #2e2e42;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-left: 30px;
`;

export const AnswerLabelSection = styled.div`
  background-color: #ededf2;
  border-radius: 5px 5px 0px 0px;
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  background-color: #ededf2;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ResetText= styled.div`
  font-size: 14px;
  text-decoration: underline;
  cursor: pointer;
`;

export const ReportQuestionText = styled.p`
  font-size: 14px;
  padding-left: 25px;

`;

export const ReportIndicateText = styled.p`
  font-size: 12px;
  padding-left: 40px;
`;

export  const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        backgroundColor: theme.palette.background.paper,
    },
    appBar: {
        backgroundColor: 'white',
        color: 'rgb(46,46,66)',
        boxShadow: 'none',
        fontWeight: 'bold',
        fontSize: '1em',
    },
    tabs: {
        '& > div > span': {
            backgroundColor: 'black',
            height: 2,
            width: '185px',
            left: "10px",
            bottom: '10px',
            fontSize: '1em',
           
        },
    },
    tab: {
        color: 'rgb(46,46,66)',
        fontWeight: 700,
        fontSize: '1em',
        paddingLeft: '0px !important',
        '&:hover': {
            '& > span:nth-of-type(1)': {
              color: 'rgb(46,46,66)',
              fontWeight: 700,
              fontSize: '1em',
              '&:after': {
                content: "",
                display: 'block',
                width: '100%',
                paddingTop: 5,
                borderBottom: '3px solid rgb(46,46,66)',
                transition: '.35s',
              }
          },
        },
        '&$selected': {
            color: 'black',
            fontWeight: theme.typography.fontWeightMedium,
            '& > span:nth-of-type(1)': {
              color: 'rgb(46,46,66)',
              fontWeight: 700,
              fontSize: '1em',
              
          }, 
        },
        '& > span': {
            color: 'rgb(46,46,66)',
            fontWeight: 700,
            fontSize: '1em',
            '&:after': {
              content: "",
              display: 'block',
              width: '100%',
              paddingTop: 5,
              borderBottom: '3px solid rgb(46,46,66)',
              transition: '.35s',
            }
        },
    },

    element: {
        width: 42,
        height: 26,
        padding: 0,
        margin: theme.spacing(1),
    }, 
    switchBase: {
        padding: 1,
        '&$checked': {
        transform: 'translateX(16px)',
        color: theme.palette.common.white,
        '& + $track': {
            backgroundColor: 'rgb(46,46,66)',
            opacity: 1,
            border: 'none',
        },
        },
        '&$focusVisible $thumb': {
        color: '#52d869',
        border: '6px solid #fff',
        },
    },
    thumb: {
        width: 24,
        height: 24,
    },
    track: {
        borderRadius: 26 / 2,
        border: `1px solid ${theme.palette.grey[400]}`,
        backgroundColor: 'gray',
        opacity: 1,
        transition: theme.transitions.create(['background-color', 'border']),
    },
    checked: {
        backgroundColor: 'rgb(46,46,66)',
    },
    focusVisible: {},
    formControl: {
        margin: theme.spacing(1),
        minWidth: 120,
    },
    grid: {
        textAlign: 'left', 
        marginTop: '10px'
    },
    closeIcon: {
      position: 'absolute',
      right: '35px',
      fontSize: '45px',
      cursor: 'pointer',
      zIndex: 500
    },
    tableStyleWithBorder: {
      // border: '1px solid rgba(224, 224, 224, 1) !important',
      '& td' :{
        border: '1px solid rgba(224, 224, 224, 1) !important',
      }
    }
}));


export const ClassCompomentstyles: any = (theme: any) => ({
  element: {
    width: 42,
    height: 26,
    padding: 0,
    margin: theme.spacing(1),
  }, 
  switchBase: {
      padding: 1,
      '&$checked': {
      transform: 'translateX(16px)',
      color: theme.palette.common.white,
      '& + $track': {
          backgroundColor: 'rgb(46,46,66)',
          opacity: 1,
          border: 'none',
      },
      },
      '&$focusVisible $thumb': {
      color: '#52d869',
      border: '6px solid #fff',
      },
  },
  thumb: {
      width: 24,
      height: 24,
  },
  track: {
      borderRadius: 26 / 2,
      border: `1px solid ${theme.palette.grey[400]}`,
      backgroundColor: 'gray',
      opacity: 1,
      transition: theme.transitions.create(['background-color', 'border']),
  },
  checked: {
      backgroundColor: 'rgb(46,46,66)',
  },
  focusVisible: {},
  formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
  },
  grid: {
      textAlign: 'left', 
      marginTop: '10px'
  }
});

export const DescriptionText = styled.p`
  font-size: 16px;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  margin-bottom: 20px;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ButtonTextStyle = styled.p`
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 700;
  line-height: 1;
  text-transform: capitalize;
  color: rgb(255,255,255);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  padding: 7px;
`;

export const LicenseTitleText = styled.h1`
  font-family: "hero-new",sans-serif;
  font-size: 2em;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-align: center;
  text-transform: none;
  color: rgb(46,46,66);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ChangePasswordLabelText = styled.p`
  font-size: 14px;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-top: 18px;
`;

export const ScreendoxRecordContainer = styled.div`
  border: 1px solid blue;
  padding: 20px;
`

export const FindAddressDescriptionText = styled.p`
  text-align: right;
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  letter-spacing: 0em;
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const FindAddressEhrDescriptionText = styled.p`
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  color: red;
  letter-spacing: 0em;
  text-transform: capitalize;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const FindAddressEhrDescriptionRedText = styled.p`
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  color: red;
  letter-spacing: 0em;
  text-transform: capitalize;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-left: 20px;
`

export const FindAddressEhrFirstNameText = styled.span`
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  letter-spacing: 0em;
  text-transform: capitalize;
  color: red;
  font-weight: bold;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`
export const FindAddressEhrLastNameText = styled.span`
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  letter-spacing: 0em;
  text-transform: capitalize;
  font-weight: bold;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const FindAddressEhrBirthText = styled.span`
  margin-top: 15px;
  margin-right: 10px;
  font-size: 16px;
  color: red;
  letter-spacing: 0em;
  text-transform: capitalize;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-left: 20px;
`

export const FindAddressEhrItemContainer = styled.div`
  padding: 10px;
  text-align: left;
  width: 50%;
  margin: 0 auto;
`

export const EhrExportDetailText = styled.p`
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: none;
  color: rgb(46,46,66);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-top: 25px;
`

export const ResetDescriptionText = styled.h1`
  font-family: "hero-new",sans-serif;
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: none;
  color: rgb(46,46,66);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const ResetTitleText = styled.h1`
  ${pageTitleTextStyle};
  letter-spacing: 0em;
  padding-bottom: 28px;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;


export const LicenseKeysSummaryTitle = styled.h1`
  font-family: "hero-new",sans-serif;
  font-size: 15.4px;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-decoration: underline;
  text-transform: none;
  color: rgb(46,46,66);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const LicenseKeysSummaryDescription= styled.h1`
  font-family: "hero-new",sans-serif;
  font-size: 14px;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-transform: none;
  color: rgb(46,46,66);
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-top: 8px;
`

export const ScreenProfileEditButtonText = styled.p`
  font-family: "hero-new",sans-serif;
  font-size: 16px;
  text-transform: none;
  font-style: normal;
  font-weight: 700;
  line-height: 1;
  color: rgb(255,255,255);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin: 5px 5px 5px 5px;
`

export const SecurityLogSettingsButtonText = styled.p`
  font-family: "hero-new",sans-serif;
  font-size: 14px;
  text-transform: none;
  font-style: normal;
  font-weight: 700;
  line-height: 1;
  color: rgb(255,255,255);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin: 5px 5px 5px 5px;
`

export const ReportIcon = styled.i`
    display: flex;
    position: relative;
    background-size: 30px;
    width: 30px;
    height: 30px;
    background-image: url(../assets/report.svg);
    &:hover {
        background-image: url(../assets/report-hover.svg);
    }
`; 

export const ManageUserTextArea = styled.textarea`
  width: 100%;
  min-height: 160px;
  border-color: #2e2e42;
  border-radius: 5px;
  padding: 18.5px 14px;
  font-weight: 800 !important;
  color: #2e2e42;
`
export const EhrLoginDateInput = styled.input`
  padding: 0 15px;
  text-align: center;
  width: auto;
  height: 40px;
  font-size: 0.9em !important;
  margin: 0;
  border: 1px solid #2e2e42;
  color: #2e2e42;
  background: rgba(255,255,255,0);
  outline: none;
  width: 100%;
  transition: border linear 0.2s, box-shadow linear 0.2s;
  border-radius: 4px;
  box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
`

export const CssrsPlusButton = styled.i`
    display: flex;
    width: 18px;
    height: 18px;
    color: #2e2e42;
    margin-right: 8px;
    font-size: 17px;
    position: relative;
    background-size: 17px;
    cursor: pointer;
    background-repeat: no-repeat, repeat;
    background-image: url(${backgroundImage});
    transition-duration: 300ms;
`; 
export const CssrsListPlusButton = styled.i`
    display: flex;
    width: 13px;
    height: 13px;
    color: #2e2e42;
    font-weight: 400   
    margin-right: 8px;   
    position: relative;   
    cursor: pointer;
    background-repeat: no-repeat, repeat;
    background-image: url(../assets/plus.svg);
    transition-duration: 300ms;
`; 
export const CssrsTextArea = styled.textarea`
  font-size: 1.2em !important;
  border: 1px solid rgb(46,46,66) !important;
  width: 100%;
  // min-height: 120px;
  background: transparent;
  color: rgb(46,46,66) !important;
  z-index: 999;
  padding-top: 6px;
  padding-bottom: 6px;
  margin-bottom: 0px;
  transition: border linear 0.2s, box-shadow linear 0.2s;
  height: auto;
  line-height: 1.3;
  display: inline-block;
  border-radius: 4px;
  box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
  width: 100%; 
  height: 100%;
  padding: 0.65em;
`

export const CssrsTextAreaWrapper = styled.p`
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  transition-duration: 300ms;
  padding: 20px 20px 0px 20px;
`

export const CssrsDateInputWrapper = styled.div`
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-align: center;
  text-transform: none;
  color: rgba(0,0,0,1);
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const CssrsDateInput = styled.input`
  display: inline-block;
  height: 2.65em;
  outline: none;
  line-height: normal;
  border-radius: 4px;
  box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
  cursor: pointer;
  padding: 0px 5px!important;
  width: 100%;
  min-height: 40px;
  font-size: 1em;
  margin: 0 !important;
  border: 1px solid rgb(46,46,66) !important;
  color: rgb(46,46,66);
  background: transparent !important;
  font-size: 1.1em !important;
  font-weight: 800;
  -webkit-appearance: none;
  max-height: 45px !important;
  z-index: 999;
}
`
export const CustomMenuItem = styled(MenuItem)`
    &.MuiMenuItem-root{        
        background: transparent;
        border: 0px solid var(--main-dark-color);
        color: #2e2e42;
        font-family: 'Hero New',sans-serif;
        font-size: 1rem;
        font-weight: normal;
        display: block;
        white-space: nowrap;        
        padding: 0px 10px;       
    }    
`
export const CustomSelect = styled(Select)`       
    .MuiSelect-outlined.MuiSelect-outlined {
        padding-right: 60px !important
      }   
  
`
export const CustomSelectTwo = styled(Select)`       
    .MuiSelect-outlined.MuiSelect-outlined {
        padding-right: 150px !important
      }   
  
`

export const ScreendoxTextInput = styled.input`
    font-size: 1.1em !important;
    font-weight: 800;
    border: 1px solid rgb(46,46,66) !important;
    font-family: 'Hero New',sans-serif;
    -webkit-appearance: none;
    border-radius: 5px;
    padding-top: 17px !important;
    cursor: text;
    background: transparent !important;
    height: 50px;
    color: rgb(46,46,66) !important;
    width: 100%;
    outline: none;
    box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%), 0 0 8px rgb(0 0 0 / 20%);
    padding-top: 0 !important;
    transition: all 0.2s;
    touch-action: manipulation;
    display: inline-block;
    line-height: normal;
    padding: 0 .65em;
    max-height: 45px !important;
    vertical-align: middle;
    margin-top: 7px;
`

export const ScreendoxTextArea = styled.textarea`
    padding: 0 .65em;
    padding-top: 6px;
    padding-bottom: 6px;
    font-family: 'Hero New',sans-serif;
    color: rgb(46,46,66) !important;
    z-index: 999;
    font-size: 1.2em !important;
    border: 1px solid rgb(46,46,66) !important;
    width: 100%;
    min-height: 120px;
    background: transparent;
    margin-top: 7px;
    display: inline-block;
    height: 2.65em;
    margin-bottom: 9px;
    border: 1px solid #ddd;
    line-height: normal;
    border-radius: 4px;
    box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
    transition: border linear 0.2s, box-shadow linear 0.2s;
    
}

`