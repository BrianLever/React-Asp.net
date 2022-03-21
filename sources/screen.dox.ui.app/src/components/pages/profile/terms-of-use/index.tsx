import { Grid, TextField, Button } from '@material-ui/core';
import RectangleCheckbox from 'components/UI/checkbox/RectangleCheckbox';
import { ProfileTitle, ProfileLargeBoldText, ProfileLargeText, ProfileSmallText , useStyles } from '../styledComponent';

const TermsOfUse = () => {
    const classes = useStyles();
    return (<>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 40 }}>  
            <RectangleCheckbox 
                name={'privacy'}
                id={'privacy'}
                changeHandler={() => {}}
                isChecked={true}
            />
            <ProfileSmallText>
                    <b>1. Privacy:</b> You acknowledge and understand that your personal information is used to create a Screendox account for your use. Your personal information will be kept private.
            </ProfileSmallText>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 40 }}>
            <RectangleCheckbox 
                name={'privacy'} 
                id={'privacy'}
                changeHandler={() => {}}
                isChecked={true}
            />
            <ProfileSmallText>
                    <b>2. Clinician Verification of Information:</b> You acknowledge and understand that Screendox relies on patient self-reporting and a qualified clinician should verify each response with each patient at the time the patient reports the information. Any diagnosis should be made on clinical grounds, taking into account how well the patient understood the questionnaire, as well as other relevant information the qualified clinician collects from the patient.
            </ProfileSmallText>
        </Grid>
        <Grid container>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 40 }}>
                <Button 
                    size="large" 
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {
                        
                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Save Changes
                    </p>
                </Button>
            </Grid>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 10}}>
                <Button 
                    size="large" 
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {

                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Cancel
                    </p>
                </Button>
            </Grid>
        </Grid></>
    )
}

export default TermsOfUse;