import React from 'react';
import Card from '../card/Card';
import Button from '../button/Button';
import classes from './ErrorModal.module.scss';
import ReactDOM from 'react-dom';

export interface IBackdropProps {
  onClick: () => void;
}

const Backdrop = (props: IBackdropProps): React.ReactElement => {
  return <div className={classes.backdrop} onClick={props.onClick} />
}

export interface IModalOverlayProps {
  onConfirm: () => void;
  title: string;
  message: string;
}

const ModalOverlay = (props: IModalOverlayProps) => {
  return (
    <Card className={classes.modal}>
        <header className={classes.header}>
          <h2>{props.title}</h2>
        </header>
        <div className={classes.content}>
          <p>{props.message}</p>
        </div>
        <footer className={classes.actions}>
          <Button type="button" color="regular" onClick={props.onConfirm}>Okay</Button>
        </footer>
    </Card>
  )
}

export interface IErrorModalProps extends IModalOverlayProps {}

const ErrorModal = (props: IErrorModalProps): React.ReactElement | null => {
  const backdropElement: Element | null = document.getElementById('backdrop-root');
  const overlayElement: Element | null = document.getElementById('overlay-root');
  if (!backdropElement || !overlayElement) { return null; }
  return (
    <>
      {
        ReactDOM.createPortal(<Backdrop onClick={props.onConfirm} />, backdropElement)
      }
      {
        ReactDOM.createPortal(
          <ModalOverlay 
            message={props.message} 
            title={props.title} 
            onConfirm={props.onConfirm} 
          />, backdropElement
        )
      }
    </>
  );
};

export default ErrorModal;
