import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import { closeModalWindow, EModalWindowKeys } from '../../../../actions/settings';
import { deleteScreenProfileRequest, screenProfileMinimumAgeUpdateRequest, 
        setCreateScreenProfileDescription, setCreateScreenProfileName, 
        setNewScreenProfileLoading, updateScreenProfileRequest,
        screenProfileFrequencyUpdateRequest
    } from 'actions/screen-profiles';
import { setScreenProfileSelectedOptionSelector } from 'selectors/screen-profiles';
import { ScreenProfileEditButtonText } from '../../styledComponents';


const EditScreenProfileActions = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const selectedScreenProfileEditOption: number  = useSelector(setScreenProfileSelectedOptionSelector);

    return (
        <Grid container spacing={1} >
            <Grid item sm={8} xs={6}>
            </Grid>
            <Grid container sm={4} xs={6}>
                <Grid item xs={12} sm={12} style={{ textAlign: 'right' }}>
                    <Button 
                        size="large"  
                        disabled={false}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                            if(selectedScreenProfileEditOption == 0) {
                                dispatch(updateScreenProfileRequest());
                            } else if(selectedScreenProfileEditOption == 1) {
                                dispatch(screenProfileMinimumAgeUpdateRequest());
                            } else if(selectedScreenProfileEditOption == 2) {
                               dispatch(screenProfileFrequencyUpdateRequest())
                            }
                        }}
                    >
                        <ScreenProfileEditButtonText>
                            Save Changes
                        </ScreenProfileEditButtonText>
                    </Button>
                    <Button 
                        size="large" 
                        disabled={false}
                        variant="contained" 
                        color="default"
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', marginLeft: 15 }}
                        onClick={() => {
                            dispatch(deleteScreenProfileRequest());
                        }}
                    >
                        <ScreenProfileEditButtonText style={{ color: '#2e2e42'}}>
                            Delete
                        </ScreenProfileEditButtonText>
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default EditScreenProfileActions;