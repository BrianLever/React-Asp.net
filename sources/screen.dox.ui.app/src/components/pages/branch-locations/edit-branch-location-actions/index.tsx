import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from '../../../../actions/settings';
import { createBranchLocationDescription, createBranchLocationName, createBranchLocationScreenProfile, deleteBranchLocationRequest, updateBranchLocationRequest } from '../../../../actions/branch-locations';
import { setBranchLocationDisabledSelector } from 'selectors/branch-locations';
import { ButtonTextStyle } from 'components/pages/styledComponents';


const EditBranchLocationsActions = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const branchLocationDisabled: boolean = useSelector(setBranchLocationDisabledSelector);

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
                        dispatch(updateBranchLocationRequest());
                    }}
                >
                    <ButtonTextStyle>
                        Save Changes
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
                        dispatch(deleteBranchLocationRequest());
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>
                        Delete
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
                        dispatch(updateBranchLocationRequest(!branchLocationDisabled));
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>
                        {!branchLocationDisabled?"Disable":"Enable"}
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
                        dispatch(closeModalWindow(EModalWindowKeys.branchLocationsEditBranchLocation));
                        dispatch(createBranchLocationDescription(''));
                        dispatch(createBranchLocationName(''));
                        dispatch(createBranchLocationScreenProfile(0))
                    }}
                >
                   <ButtonTextStyle style={{ color: '#2e2e42' }}>
                        Cancel
                    </ButtonTextStyle>
                </Button>
            </Grid>
        </Grid>
    )
}

export default EditBranchLocationsActions;