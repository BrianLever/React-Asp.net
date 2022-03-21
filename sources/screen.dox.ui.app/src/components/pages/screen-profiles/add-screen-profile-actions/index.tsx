import React from 'react';
import { useDispatch } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from '../../../../actions/settings';
import { createNewBranchLocationRequest, setNewBranchLocationLoading } from '../../../../actions/branch-locations';
import { createNewScreenProfileRequest, setCreateScreenProfileDescription, setCreateScreenProfileName, setNewScreenProfileLoading } from 'actions/screen-profiles';
import { ButtonTextStyle } from 'components/pages/styledComponents';

const AddScreenProfileActions = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    return (
        <Grid container spacing={1} style={{ fontSize: 16 }}>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth 
                    disabled={false}
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {
                        dispatch(createNewScreenProfileRequest());
                        dispatch(setNewScreenProfileLoading(true));
                    }}
                >
                    <ButtonTextStyle>
                        Create
                    </ButtonTextStyle>
                </Button>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth
                    disabled={false}
                    variant="contained" 
                    color="default"
                    className={classes.removeBoxShadow}
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                    onClick={() => {
                        dispatch(closeModalWindow(EModalWindowKeys.screenProfileAdd));
                        dispatch(setCreateScreenProfileDescription(''));
                        dispatch(setCreateScreenProfileName(''))
                        dispatch(setNewScreenProfileLoading(false));
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42'}}>
                        Cancel
                    </ButtonTextStyle>
                </Button>
            </Grid>
        </Grid>
    )
}

export default AddScreenProfileActions;