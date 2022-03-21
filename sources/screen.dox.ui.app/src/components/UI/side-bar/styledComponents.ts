import { makeStyles, Theme, createStyles, alpha  } from '@material-ui/core/styles';
import styled, { css }  from 'styled-components';


export const drawerWidth = 256;

export const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    grow: {
      flexGrow: 1,
    },
    paper: {
      border: '1px solid',
      padding: theme.spacing(1),
      backgroundColor: theme.palette.background.paper,
    },
    profileText: {
      fontSize: '0.8em',
      fontStyle: 'normal',
      fontWeight: 700,
      lineHeight: 1,
      color: '#2e2e42',
    },
    title: {
      display: 'none',
      [theme.breakpoints.up('lg')]: {
        display: 'block',
      },
    },
    search: {
      position: 'relative',
      borderRadius: theme.shape.borderRadius,
      color: 'black',
      backgroundColor: alpha('#f4f6f8', 0),
      width: '100vw',
      '&:focus': {
        backgroundColor: alpha(theme.palette.common.white, 0.80),
      },
      '&:visited': {
        backgroundColor: alpha(theme.palette.common.white, 0.80),
      },
      marginRight: theme.spacing(2),
      marginLeft: 0,
      [theme.breakpoints.up('lg')]: {
        marginLeft: theme.spacing(3),
        width: '100%',
      },
    },
    searchIcon: {
      padding: theme.spacing(0, 2),
      height: '100%',
      position: 'absolute',
      pointerEvents: 'none',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      color: 'black'
    },
    inputRoot: {
      color: 'inherit',
      width: '100%'
    },
    inputInput: {
      padding: theme.spacing(1, 1, 1, 0),
      paddingLeft: `calc(16px; + ${theme.spacing(4)}px)`,
      transition: theme.transitions.create('width'),
      backgroundColor: 'transparent',
      width: '100%',
      '&:focus': {
        borderRadius: '100em',
        width: '100%',
        backgroundColor: alpha(theme.palette.common.white, 0.80),
      },
      [theme.breakpoints.up('lg')]: {
        width: '100%',
      },
    },
    sectionDesktop: {
      display: 'flex',
      color: 'back',
      justifyContent: 'space-between',
      [theme.breakpoints.up('lg')]: {
        display: 'flex',
      },
    },
    sectionMobile: {
      display: 'flex',
      [theme.breakpoints.up('lg')]: {
        display: 'none',
      },
    },
    root: {
      display: 'flex',
    },
    drawer: {
      border: 0,
      [theme.breakpoints.up('lg')]: {
        width: drawerWidth,
        flexShrink: 0,
      },
      [theme.breakpoints.down('lg')]: {
        width: 0,
      },
    },
    drawerSimple: {
      border: 0,
      [theme.breakpoints.up('lg')]: {
        flexShrink: 0,
      },
    },
    appBar: {
      [theme.breakpoints.up('xl')]: {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: drawerWidth,
      },
      backgroundColor: 'transparent',
      boxShadow: 'none',
    },
    menuButton: {
      marginRight: theme.spacing(1),
      [theme.breakpoints.up('xl')]: {
        display: 'none',
      },
    },
    // necessary for content to be below app bar
    toolbar: theme.mixins.toolbar,
    toolbarWithoutBorder: {
      border: 0,
      minHeight: '134px',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center'
    },
    drawerPaper: {
      border: 0,
      width: drawerWidth,
      zIndex: 0,
    },
    content: {
      flexGrow: 1,
      padding: theme.spacing(3),
      width: 'calc(100vw - 260px)',
      height: '100vh',
      overflowX: 'auto',
      [theme.breakpoints.down('lg')]: {
        width: '100vw',
      },
    },
    closeIcon: {
      position: 'absolute',
      right: '45px',
      fontSize: '45px',
      cursor: 'pointer',
      zIndex: 500,
      top: 45
    }
  }),
);

export const ScreendoxAboutModalTitle = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 32px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: rgb(46,46,66);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const ScreendoxAboutModalDesriptionText = styled.h1`
  font-family: inherit;
  font-size: 16px;
  font-style: normal;
  font-weight: 400;
  line-height: 1.4;
  letter-spacing: 0em;
  text-transform: none;
  color: rgb(46,46,66);
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  margin-top: 10px;
`

export const ScreendoxAboutModalTrademarksText = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 25px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: rgb(46,46,66);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin-top: 30px;
`

export const ScreendoxAboutModalSupportLinkText = styled.span`
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 700;
  line-height: 1;
  color: rgb(255,255,255);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  text-transform: none;
`