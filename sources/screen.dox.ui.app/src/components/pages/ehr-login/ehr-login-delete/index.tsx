import React, { ChangeEvent } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Button } from '@material-ui/core';
import { LicenseTitleText, ButtonTextStyle, useStyles } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from 'actions/settings';
import CloseIcon from '@material-ui/icons/Close';
import { getEhrLoginSelectdIdSelector } from 'selectors/ehr-login';
import { ehrLoginDeleteRequest } from 'actions/ehr-login';


const EhrLoginDelete = (): React.ReactElement => {

    const dispatch = useDispatch();
    const customClasses = useStyles();
    const selectedId = useSelector(getEhrLoginSelectdIdSelector);

    return (
        <div style={{ textAlign: 'center' }}>
            <Grid item sm={12}>
                <CloseIcon className={customClasses.closeIcon} 
                    style={{ top: 10, right: 10 }}
                    onClick={() =>{
                        dispatch(closeModalWindow(EModalWindowKeys.ehrLoginDelete));
                    }
                }/>
            </Grid>
            <Grid item sm={12}><LicenseTitleText>Are you sure you want to delete this item?</LicenseTitleText></Grid>
            <Grid item sm={12} style={{ padding: 30 }}>
                <Button 
                    size="large" 
                    disabled={false}
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42', marginRight: 10 }}
                    onClick={() => {
                        if(selectedId) {
                            dispatch(ehrLoginDeleteRequest(selectedId));
                        }
                    }}
                >
                    <ButtonTextStyle style={{ color: '#fff' }}>
                        YES, PROCEED
                    </ButtonTextStyle>
                </Button>
                <Button 
                    size="large" 
                    disabled={false}
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: 'transparent', border: '2px solid #2e2e42' }}
                    onClick={() => {
                        dispatch(closeModalWindow(EModalWindowKeys.ehrLoginDelete))
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>
                        NO, GO BACK
                    </ButtonTextStyle>
                </Button>
            </Grid>
        </div>  
    )
}

export default EhrLoginDelete;
