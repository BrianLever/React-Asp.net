import moment from 'moment';

export const convertDateToStringFormat = (date: string | Date | never, f?: string): string => {
    if (!date) {
      return '';
    }
    const todayDay = moment(new Date().toISOString()).dayOfYear();
    const givvenDate = date ? moment(date).dayOfYear() : 0;
    const isGivvenDateIsToday = givvenDate === todayDay;
    if (isGivvenDateIsToday) {
      return moment(date).format("HH:MM");
    } else {
      if (f) {
        return moment(date).format(f);
      } else {
        return moment(date).format("MM/DD/YYYY HH:MM");
      }
    }
  };

  export const convertDateTimeOffsetToStringFormat = (date: string | Date | never | null, f?: string): string => {
    if (!date) {
      return '';
    }
      if (f) {
        return moment(date).format(f);
      } else {
        return moment(date).format("MM/DD/YYYY HH:MM Z");
      }
  };

  export const convertDate = (date: string | null | undefined) => {
    
    if (!date) {
      return '';
    }

    if(date) {
      try {
        var dateFormat = new Date(date);
        return new Intl.DateTimeFormat('en-US', {year: 'numeric', month: '2-digit',day: '2-digit'}).format(dateFormat)
      } catch(e) {
        return '';
      }
    }
  }
  
