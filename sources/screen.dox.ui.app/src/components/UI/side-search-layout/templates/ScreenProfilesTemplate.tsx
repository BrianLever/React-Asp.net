import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, FormControl, Grid, Select, TextField } from '@material-ui/core';
import { TitleText, TitleTextH2, ContainerBlock } from './styledComponents';
import classes from  '../templates.module.scss';
import { getListBranchLocationsSelector, getScreeningProfileListSelector } from '../../../../selectors/shared';
import { 
    getListBranchLocationsRequest, getScreeningProfileListRequest, TBranchLocationsItemResponse, TScreeningProfileItemResponse,
} from '../../../../actions/shared';
import { setFilterByNameAction, getScreenProfileListRequest, clearSearchParamsRequest } from 'actions/screen-profiles';
import { screenProfileFilterByNameSelector } from 'selectors/screen-profiles';
import { ButtonText } from '../styledComponents'; 

const ScreenProfilesTemplate = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const name = useSelector(screenProfileFilterByNameSelector);
    React.useEffect(() => {
       
    }, [])


    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search Screen Profiles
                    </TitleText>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Screen Profile Name
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        id="search-branch-location"
                        variant="outlined"
                        value={name}
                        onChange={e => {console.log(e.target.value);
                            e.stopPropagation();
                            dispatch(setFilterByNameAction(e.target.value))
                        }}

                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(getScreenProfileListRequest())
                             }
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        disabled={false}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', textTransform: 'none', marginTop: 30}}
                        onClick={() => {
                            dispatch(getScreenProfileListRequest())
                        }}
                    >
                        <ButtonText>Search</ButtonText>
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
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', textTransform: 'none', }}
                        onClick={() => {
                            dispatch(clearSearchParamsRequest())
                            dispatch(getScreenProfileListRequest())
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Cancel</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default ScreenProfilesTemplate;