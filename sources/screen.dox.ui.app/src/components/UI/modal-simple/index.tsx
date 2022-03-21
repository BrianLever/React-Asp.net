import React from 'react';
import { Grid, Card, Button } from '@material-ui/core';
import Modal from '@material-ui/core/Modal';
import { useSelector } from 'react-redux';
import { getModalWindowObjectSelector } from '../../../selectors/settings';
import classes from './Modal.module.scss';


function getModalStyle() {
  return {
    top: `45%`,
    left: `45%`,
    transform: `translate(-45%, -45%)`,
  };
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

export type TSimpleModal = {
    onConfirm: () => void;
    title: string;
    content: any;
    uniqueKey: string;
}


const SimpleModal = (props: TSimpleModal): React.ReactElement => {
    const  { uniqueKey = '' } = props;
    // const [modalStyle] = React.useState(getModalStyle);
    const modalsObject: { [k:string]: boolean } = useSelector(getModalWindowObjectSelector);

    const body = (
        <div  className={classes.paper}>
            <Card className={classes.modal}>
                <Grid container spacing={1}>
                    <Grid xs={8} md={8}>
                    <header className={classes.header}>
                        <h2>{props.title}</h2>
                    </header>
                    <div className={classes.content}>
                        { props.content }
                    </div>
                    </Grid>
                    <Grid xs={4} md={8}>
                        <div className={classes.actions}>
                            <Button type="button" onClick={props.onConfirm}></Button>
                        </div>
                    </Grid>
                </Grid>
            </Card>
        </div>
    );

  return (
    <Modal
        open={isModalShow(modalsObject, uniqueKey)}
        onClose={() =>  {}}
        aria-labelledby="simple-modal-title"
        aria-describedby="simple-modal-description"
    >
    {body}
    </Modal>
  );
}

export default SimpleModal;