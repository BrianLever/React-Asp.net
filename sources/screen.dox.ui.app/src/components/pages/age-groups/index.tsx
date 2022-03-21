
import { getAgeGroupRequest, ageGroupsResponseItem, updateAgeGroupRequest } from 'actions/age-groups';
import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { IRootState } from '../../../states';
import { FormControl, Select, TextField, Grid, Button, CircularProgress } from '@material-ui/core';
import { ContentContainer, TitleText, ResetText } from '../styledComponents';
import { setAgeGroupValue } from 'actions/age-groups';
import  { IsAgeGroupsListLoading } from 'selectors/age-groups'; 
import customClass from '../pages.module.scss';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
import CustomAlert from 'components/UI/alert';


export interface AgeGroupsProps {
    getAgeGroups?: () => void;
    ageGroups: ageGroupsResponseItem;
    setAgeGroupValue?: (value: string) => void;
    updateAgeGroupRequest?: () => void;
    isLoading: boolean;
    setCurrentPage?: (k: string, p: string) => void;
}

class AgeGroups extends React.Component<AgeGroupsProps> {

    public componentDidMount() {
        this.props.getAgeGroups && this.props.getAgeGroups();
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.AGE_GROUPS, ERouterUrls.AGE_GROUPS);
    }

    render(): React.ReactElement {
        return (
          <ContentContainer>
            <CustomAlert />
            {this.props.isLoading?<CircularProgress disableShrink={false} className={customClass.circularLoadingStyle}/>:  
            <Grid container spacing={1} >
                <Grid item xs={12}>
                    <TitleText>{this.props.ageGroups.Value.Name}</TitleText>
                </Grid>
                <Grid item xs={12} justifyContent={'space-around'}>
                    <FormControl fullWidth variant="outlined">
                        <TextField
                            margin="dense"
                            variant="outlined"
                            value={this.props.ageGroups.Value.Value}
                            onChange={(e) => this.props.setAgeGroupValue && this.props.setAgeGroupValue(e.target.value)}
                        />
                    </FormControl>
                </Grid>
                <Grid item xs={12}>
                    <ResetText onClick={() => this.props.setAgeGroupValue && this.props.setAgeGroupValue(this.props.ageGroups.DefaultValue) }>Reset to Default</ResetText>
                </Grid>
                <Grid item xs={12}>
                    <Button 
                        size="large"  
                        disabled={false}
                        variant="contained" 
                        color="primary" 
                        className={customClass.blackButtonStyle}
                        onClick={() => this.props.updateAgeGroupRequest && this.props.updateAgeGroupRequest()}
                    >
                        Save Changes
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ marginTop: 20 }}>
                    <TitleText>{'Preview Age Report groups'}</TitleText>
                </Grid>
                <Grid item xs={12}>
                    {this.props.ageGroups.Labels && this.props.ageGroups.Labels.map((item, i) => (
                        <p key={i}>
                            {item}
                        </p>
                    ))}
                </Grid>
            </Grid>}
          </ContentContainer>
        )
    }
}


const mapStateToPtops = (state: IRootState) => ({
    ageGroups: state.ageGroups,
    isLoading: IsAgeGroupsListLoading(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getAgeGroups: () => dispatch(getAgeGroupRequest()),
        setAgeGroupValue: (value: string) => dispatch(setAgeGroupValue(value)),
        updateAgeGroupRequest: () => dispatch(updateAgeGroupRequest()),
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }  
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(AgeGroups);