import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import CssBaseline from '@material-ui/core/CssBaseline';
import Drawer from '@material-ui/core/Drawer';
import Hidden from '@material-ui/core/Hidden';
import Toolbar from '@material-ui/core/Toolbar';
import { useTheme } from '@material-ui/core/styles';
// import Badge from '@material-ui/core/Badge';
import MenuItem from '@material-ui/core/MenuItem';
import Menu from '@material-ui/core/Menu';
import MenuIcon from '@material-ui/icons/Menu';
import AccountCircle from '@material-ui/icons/AccountCircle';
// import NotificationsIcon from '@material-ui/icons/Notifications';
import { Typography, Button, Grid } from '@material-ui/core';
import SettingsIcon from '@material-ui/icons/Settings';
import { DropDownTree } from '../dropdown';
import { IPage } from '../../../common/types/page';
import { useDispatch, useSelector } from 'react-redux';
import { getFullNameSelector } from '../../../selectors/profile';
import useMediaQuery from '@material-ui/core/useMediaQuery';
import ScreendoxDivider from '../divider';
import { DROPDOWN_ELEMENTS, TOP_NAVIGATION_ITEMS } from './helper'
import { useStyles, ScreendoxAboutModalTitle, ScreendoxAboutModalDesriptionText, ScreendoxAboutModalTrademarksText, ScreendoxAboutModalSupportLinkText } from './styledComponents';
import * as appConfig from '../../../config/app.json';
import SidebarPoperContent from './sidebar-poper-content';
import { getAppVersion } from '../../../selectors/settings';
import ProfileMenuContent from './profile-menu-content';
import { TSidebarPoperContentProps } from './sidebar-poper-content';
import { closeModalWindow, EModalWindowKeys } from 'actions/settings';
import { ScreendoxAboutModal } from 'components/UI/modal';
import CloseIcon from '@material-ui/icons/Close';
import { useHistory } from 'react-router';

interface DrawerProps extends IPage {
  window?: () => Window;
  children: any;
}

const CustomDrawer = (props: DrawerProps) => {
  const { window } = props;
  const classes = useStyles();
  const fullName = useSelector(getFullNameSelector);
  const appVersion: string = useSelector(getAppVersion);
  const theme = useTheme();
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState<null | HTMLElement>(null);
  const [profileAnchorEl, setProfileAnchorEl] = React.useState<null |  HTMLElement>(null);
  const dispatch = useDispatch();
  const history = useHistory();
  const isMenuOpen = Boolean(anchorEl);
  const isProfileMenuOpen = Boolean(profileAnchorEl);
  const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);
  const matches = useMediaQuery('(max-width:960px)');

  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMobileMenuClose = () => {
    setMobileMoreAnchorEl(null);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    handleMobileMenuClose();
  };


  const handleProfileUserMenuClose = () => {
    setProfileAnchorEl(null);
  }

  const handleProfileUserMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setProfileAnchorEl(event.currentTarget);
  };
  
  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const menuId = 'primary-search-account-menu';
  
  const drawer = (
    <div>
      <div className={`${classes.toolbar} ${classes.toolbarWithoutBorder}`} >
        <img 
          className="details-image" 
          src="../assets/logo.svg" 
          width="113" 
          height="30" 
          draggable="false" 
          alt="" 
        />
      </div>
      <DropDownTree 
        {...props}
        tree={DROPDOWN_ELEMENTS} />
    </div>
  );


 const renderRef = React.useRef(null);
  const renderMenu = (
    <Menu
      anchorEl={anchorEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      marginThreshold={55}
      id={menuId}
      keepMounted
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={isMenuOpen}
      onClose={handleMenuClose}
    >
      <div ref={renderRef}>
        <SidebarPoperContent 
          titlesarr={[{ name: 'ACCOUNT' }, { name: 'SETTINGS' }]}
          itemsarr={TOP_NAVIGATION_ITEMS}
          onClick={handleMenuClose}
        />
      </div>
    </Menu>
  );

  const profileMenuId = 'primary-search-profile-menu';

  const renderProfileMenu = (
    <Menu
      anchorEl={profileAnchorEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      marginThreshold={55}
      id={profileMenuId}
      keepMounted
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={isProfileMenuOpen}
      onClose={handleProfileUserMenuClose}
    >
      <div>
      <ProfileMenuContent 
        onClick={handleProfileUserMenuClose}
      />
     </div>
    </Menu>
  );

  const mobileMenuId = 'primary-search-account-menu-mobile';
  const renderMobileMenu = (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >
      <MenuItem onClick={handleProfileMenuOpen}>
        <Button
          aria-label="account of current user"
          aria-controls="primary-search-account-menu"
          aria-haspopup="true"
          color="inherit"
        >
          <AccountCircle />
        </Button>
        <p>Profile</p>
      </MenuItem>
    </Menu>
  );

  const ScreendoxAboutModalComponent = (): React.ReactElement => {
    return (
      <Grid container>
        <Grid item sm={3}>

        </Grid>
        <Grid item sm={9}>
          <Grid item sm={12}>
              <ScreendoxAboutModalTitle>Screendox</ScreendoxAboutModalTitle>
              <CloseIcon className={classes.closeIcon} onClick={() => dispatch(closeModalWindow(EModalWindowKeys.screendoxAbout))}/>
          </Grid>
          <Grid item sm={12}>
            <ScreendoxAboutModalDesriptionText>{ appVersion ?  `Version: ${appVersion}` : appConfig.APP_VERSION }</ScreendoxAboutModalDesriptionText>
          </Grid>
          <Grid item sm={12}>
            <ScreendoxAboutModalTrademarksText>Screendox Trademarks & Copyrights</ScreendoxAboutModalTrademarksText>
          </Grid>
          <Grid item sm={12}>
            <ScreendoxAboutModalDesriptionText>Screendox is the property of J.L. Ward Associates, Inc. and is registered with and protected by the United States Copyright Office and the United States Patent and Trademark Office.</ScreendoxAboutModalDesriptionText>
            <ScreendoxAboutModalDesriptionText>Screendox is not sold. Rather, copies are licensed on an annual basis. Per the licensing agreement, J.L. Ward Associates, Inc. permits licensee to install and use Screendox.</ScreendoxAboutModalDesriptionText>
            <ScreendoxAboutModalDesriptionText>The Screendox name and stylized “X” logo is a registered trademark (No. 5,231,457 and 6,078,403) issued by the United States Patent and Trademark Office and is owned by J.L. Ward Associates, Inc.</ScreendoxAboutModalDesriptionText>
          </Grid>
          <Grid item sm={12}>
            <Button 
                size="large" 
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: '#2e2e42', marginTop: 30, borderRadius: 5 }}
                onClick={() => {
                    
                }}
            >
                <a style={{ color: '#fff', textDecoration: 'none' }} href={appConfig.SUPPORT_LINK} target="_blank">
                  <ScreendoxAboutModalSupportLinkText>Support</ScreendoxAboutModalSupportLinkText>
                </a>
            </Button>
          </Grid>
          <Grid item sm={12} style={{ marginTop: 50 }}>
            <ScreendoxAboutModalDesriptionText>©{new Date().getFullYear()}, J.L. Ward Associates, Inc. All rights reserved.</ScreendoxAboutModalDesriptionText>
          </Grid>
        </Grid>
      </Grid>
    )
  }


  const container = window !== undefined ? () => window().document.body : undefined;

  return (
    <div className={classes.root}>
      <CssBaseline />
      <AppBar position="fixed" className={classes.appBar} >
        <Toolbar>
          <Button
            className={classes.menuButton}
            color="default"
            onClick={handleDrawerToggle}
          >
            <MenuIcon 
              fontSize="large"
              style={{
                  backgroundColor: '#2e2e42',
                  color: '#fff',
                  borderRadius: '3px',
                  letterSpacing: 0,
                  lineHeight: 1,
              }} 
            />
          </Button>
          <div className={classes.grow} />
          <div className={classes.sectionDesktop}>
              <Button aria-label="name" color="default" onClick={handleProfileUserMenuOpen}>
                <Typography variant="subtitle2" noWrap={true} className={classes.profileText}> 
                  { fullName } 
                </Typography>
              </Button>
            <ScreendoxDivider/>
            <Button
              onClick={handleProfileMenuOpen}
              color="default"
            >
              <SettingsIcon/>
            </Button>
            <ScreendoxDivider/>
            <div 
              className={classes.profileText} 
              style={{ 
                display: 'flex', 
                alignItems: 'center', 
                justifyContent: 'center' 
              }}
            >
              { appVersion ?  `Version: ${appVersion}` : appConfig.APP_VERSION }
            </div>
          </div>
        </Toolbar>
      </AppBar>
      {renderMobileMenu}
      {renderMenu}
      {renderProfileMenu}
      <nav className={ matches ? classes.drawerSimple : classes.drawer} aria-label="mailbox folders">
        <Hidden lgUp implementation="css">
          <Drawer
            container={container}
            style={{ border: 0 }}
            variant="temporary"
            anchor={theme.direction === 'rtl' ? 'right' : 'left'}
            open={mobileOpen}
            onClose={handleDrawerToggle}
            classes={{
              paper: classes.drawerPaper,
            }}
            ModalProps={{ keepMounted: true }}
          >
            {drawer}
          </Drawer>
        </Hidden>
        <Hidden lgDown implementation="css">
          <Drawer
            style={{ border: 0 }}
            classes={{
              paper: classes.drawerPaper,
            }}
            variant="permanent"
            open
          >
            {drawer}
          </Drawer>
        </Hidden>
      </nav>
      <main className={ matches ? classes.drawerSimple : classes.content}>
        <div className={classes.toolbar} />
        { props.children }
      </main>
      <ScreendoxAboutModal
          uniqueKey={EModalWindowKeys.screendoxAbout}
          content={<ScreendoxAboutModalComponent />}
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.screendoxAbout));
          }}
      />
    </div>
  );
}

export default CustomDrawer;