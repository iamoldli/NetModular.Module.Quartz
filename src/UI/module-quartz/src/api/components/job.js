import module from '../../module'

export default name => {
  const root = `${module.code}/${name}/`
  const crud = $http.crud(root)
  const urls = {
    jobSelect: root + 'JobSelect',
    pause: root + 'Pause',
    resume: root + 'Resume',
    stop: root + 'Stop',
    log: root + 'log',
    addHttpJob: root + 'AddHttpJob',
    authTypeSelect: root + 'AuthTypeSelect',
    contentTypeSelect: root + 'contentTypeSelect',
    editHttpJob: root + 'editHttpJob',
    updateHttpJob: root + 'updateHttpJob',
    jobHttpDetails: root + 'jobHttpDetails'
  }

  const jobSelect = moduleCode => {
    return $http.get(urls.jobSelect, { moduleCode })
  }

  const pause = id => {
    return $http.post(urls.pause + '?id=' + id)
  }

  const resume = id => {
    return $http.post(urls.resume + '?id=' + id)
  }

  const stop = id => {
    return $http.post(urls.stop + '?id=' + id)
  }

  const log = params => {
    return $http.get(urls.log, params)
  }

  const addHttpJob = params => {
    return $http.post(urls.addHttpJob, params)
  }

  const authTypeSelect = () => {
    return $http.get(urls.authTypeSelect)
  }

  const contentTypeSelect = () => {
    return $http.get(urls.contentTypeSelect)
  }

  const editHttpJob = id => {
    return $http.get(urls.editHttpJob, { id })
  }

  const updateHttpJob = params => {
    return $http.post(urls.updateHttpJob, params)
  }
  const jobHttpDetails = id => {
    return $http.get(urls.jobHttpDetails, { id })
  }
  return {
    ...crud,
    jobSelect,
    pause,
    resume,
    stop,
    log,
    addHttpJob,
    authTypeSelect,
    contentTypeSelect,
    editHttpJob,
    updateHttpJob,
    jobHttpDetails
  }
}
