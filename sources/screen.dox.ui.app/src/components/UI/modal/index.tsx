import React from 'react';
import ReactDOM from 'react-dom';
import { useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import Card from '../card/Card';
import classes from './Modal.module.scss';
import { getModalWindowObjectSelector } from '../../../selectors/settings';
import { useEffect } from 'react';
import CloseIcon from '@material-ui/icons/Close';


export interface IBackdropProps {
  onClick: () => void;
}

const Backdrop = (props: IBackdropProps): React.ReactElement => {
  return <div className={classes.backdrop} onClick={props.onClick} />
}

export interface IModalOverlayProps {
  onConfirm: () => void;
  title: string;
  content: any;
  actions: any;
}

const ReqularModalOverlay = (props: IModalOverlayProps) => {
  return (
    <Card className={classes.modal}>
      <Grid container spacing={1} justifyContent="space-around">
        <Grid item xs={8} md={8}>
          <header className={classes.header}>
            <h2>{props.title}</h2>
            <CloseIcon className={classes.closeIcon} 
              style={{ top: 10, right: 10, fontSize: 45 }}
              onClick={() =>{
                  props.onConfirm();
              }
            }/>
          </header>
          <div className={classes.content}>
            { props.content }
          </div>
        </Grid>
        <Grid item xs={4} md={4} style={{ alignItems: 'center', justifyContent: 'center' }}>
          <div className={classes.actions}>
            { props.actions }
          </div>
        </Grid>
      </Grid>
    </Card>
  )
}

export interface IErrorModalProps extends IModalOverlayProps {
  uniqueKey: string;
}


const LicenseKeysModalOverlay = (props: IModalOverlayProps) => {
    return (
      <Card className={classes.miniModal}>
            <div className={classes.content}>
              { props.content }
            </div>
      </Card>
    )
}
const isModalShow = (o: { [k:string]: boolean }, k: string): boolean => {
  if (!k || !o) {
    return false;
  }
  if (!o[k]) {
    return false;
  }
  return true;
}

const ScreendoxModal = (props: IErrorModalProps): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  
  if (!backdropElement || !overlayElement) { return null; }

  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<Backdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <ReqularModalOverlay 
                  content={props.content} 
                  title={props.title}
                  actions={props.actions}
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
};

export default ScreendoxModal;

const ScreenProfileBackdrop = (props: IBackdropProps): React.ReactElement => {
  return <div className={classes.backdrop} onClick={props.onClick} />
}


const ScreenProfileEditModalOverlay = (props: IModalOverlayProps) => {
  return (
    <Card className={classes.screenModal}>
      <Grid container spacing={1} justifyContent="space-around">
        <Grid item xs={12} md={12}>
          <Grid className={classes.screenProfileHeaderBorderStyle} item >
          <header className={classes.header}>
            <h2>{props.title}</h2>
            <CloseIcon className={classes.closeIcon} 
                style={{ top: 10, right: 10, fontSize: 45 }}
                onClick={() =>{
                    props.onConfirm();
                }
            }/>
          </header>
          </Grid>
          <Grid item xs={12} md={12} style={{ alignItems: 'center', justifyContent: 'center' }}>
            <div className={classes.actions}>
              { props.actions }
            </div>
          </Grid>
          <div className={classes.content}>
            { props.content }
          </div>
        </Grid>
      </Grid>
    </Card>
  )
}

export const ScreenProfileEditModal = (props: IErrorModalProps): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);

  const backdropElement: Element | null = document.getElementById('d-backdrop-root');
  const overlayElement: Element | null = document.getElementById('d-overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<ScreenProfileBackdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <ScreenProfileEditModalOverlay 
                  content={props.content} 
                  title={props.title}
                  actions={props.actions}
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
};


export const LicenseKeysMiniModal = (props: IErrorModalProps): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);

  const backdropElement: Element | null = document.getElementById('d-backdrop-root');
  const overlayElement: Element | null = document.getElementById('d-overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<Backdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <LicenseKeysModalOverlay 
                  content={props.content} 
                  title={props.title}
                  actions={props.actions}
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
};


const EhrExportModalOverlay = (props: IModalOverlayProps) => {
  return (
    <Card className={classes.ehrExportModal}>
      <Grid container spacing={1} justifyContent="space-around">
        <Grid item xs={12} md={12}>
          {props.content}
        </Grid>
      </Grid>
    </Card>
  )
}

export const EhrExportModal = (props: IErrorModalProps): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<ScreenProfileBackdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <EhrExportModalOverlay 
                  content={props.content} 
                  title={props.title}
                  actions={props.actions}
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
}

const EhrExportScreenDoxInformationModalOverlay = (props: { onConfirm: () => void }) => {
  return (
    <Card className={classes.ehrExportScreenRecordInformationModal}>
      <CloseIcon className={classes.closeIcon} onClick={() => props.onConfirm()}/>
      <Grid container spacing={1} justifyContent="space-around">
        <Grid item xs={12} md={12}>
          <h1 className={classes.informationTitle}>Information</h1>
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
              The Screendox record is the information the patient entered during their screen. If any of the information (e.g., name, date of birth, phone number, address) is different than the EHR record, the text in the EHR record will appear in RED.
          </div>  
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
           Verify with the patient the correct information.
          </div>
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
            If the Screendox record is incorrect, change the information in the Screendox box and click Save to Screendox.
          </div>
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
            If the information is correct in the Screendox record it will replace the EHR information. NOTE: Screendox will not replace name or date of birth in the EHR record. 
            If this information is incorrect in the EHR, it must be corrected in the EHR.
          </div>
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
            Blank boxes in the Screendox record means the information was not collected during the screen.
          </div>
        </Grid>
        <Grid item xs={12} md={12}>
          <div className={classes.informationDescription}>
            Select matching EHR record and click next.
          </div>
        </Grid>
      </Grid>
    </Card>
  )
}


export const ScreendoAboutModalOverlay = (props: { onConfirm: () => void, content: any}) => {
  return (
      <Card className={classes.screendoxAboutModal}>
          {props.content}
      </Card>)
}


export const EhrExportScreenDoxInformationModal = (props: { uniqueKey: string, onConfirm: () => void }): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<ScreenProfileBackdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <EhrExportScreenDoxInformationModalOverlay 
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
}


export const ScreendoxAboutModal = (props: { uniqueKey: string, onConfirm: () => void, content: any }): React.ReactElement | null =>{
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<ScreenProfileBackdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <ScreendoAboutModalOverlay 
                  onConfirm={props.onConfirm} 
                  content={props.content}
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
}

export const ManageUsersModalOverlay = (props: IModalOverlayProps) => {
  return (
    <Card className={classes.manageUsersModal}>
      <Grid item sm={12} style={{ padding: 15 }}>
          <header className={`${classes.header} ${classes.screenProfileHeaderBorderStyle}`}>
            <h2>{props.title}</h2>
            <CloseIcon className={classes.closeIcon} 
              style={{ top: 20, right: 20, fontSize: 45 }}
              onClick={() =>{
                  props.onConfirm();
              }
            }/>
          </header>
      </Grid>
      <Grid container spacing={1} justifyContent="space-around">
        <Grid item xs={12} md={9}>
          <div className={classes.content}>
            { props.content }
          </div>
        </Grid>
        <Grid item xs={12} md={3} style={{ alignItems: 'center', justifyContent: 'center' }}>
          <div className={classes.actions}>
            { props.actions }
          </div>
        </Grid>
      </Grid>
    </Card>
  )
}


export const ManageUsersModal = (props: IErrorModalProps): React.ReactElement | null => {
  const  { uniqueKey = '' } = props;
  const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  
  if (!backdropElement || !overlayElement) { return null; }

  return (
    <>
      {
        isModalShow(modalsObject, uniqueKey) ? (
          <>
            {
              ReactDOM.createPortal(<Backdrop onClick={props.onConfirm} />, backdropElement)
            }
            {
               ReactDOM.createPortal(
                <ManageUsersModalOverlay 
                  content={props.content} 
                  title={props.title}
                  actions={props.actions}
                  onConfirm={props.onConfirm} 
                />, backdropElement
              )
            }
          </>
        ) : null
      }
    </>
  );
};